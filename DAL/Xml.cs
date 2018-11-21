using DAL.Properties;
using Microsoft.Office.Interop.Outlook;
using ProdusisBD;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace DAL
{
    public class Xml
    {
        /// <summary>
        /// Abre o email e salva todos os anexos na pasta de notas fiscais
        /// </summary>
        public void ExtrairAnexosDeEmail(string nomeArquivo, Application Outlook)
        {
            MailItem email = (MailItem)Outlook.Session.OpenSharedItem(nomeArquivo);
            try
            {
                int i = 0;
                foreach (Attachment anexo in email.Attachments)
                {
                    string nomeXML = PastasXml.Default.PastaNFs + "\\" + anexo.FileName;
                    while (File.Exists(nomeXML))
                    {
                        nomeXML = nomeXML.Insert(nomeXML.Length - 4, i.ToString());
                    }
                    anexo.SaveAsFile(nomeXML);
                    i++;
                }
                email.Close(OlInspectorClose.olDiscard);
            }
            catch
            {
                email.Close(OlInspectorClose.olDiscard);
            }
        }

        /// <summary>
        /// Importa os dados de xml de manifestos na pasta padrão
        /// </summary>
        public bool LerManifesto(string nomeArquivo)
        {
            try
            {
                XmlDocument manifesto = new XmlDocument();
                manifesto.Load(nomeArquivo);

                DocumentosBD docBD = new DocumentosBD();
                var result = manifesto.GetElementsByTagName("Value");
                Manifestos lido = new Manifestos
                {
                    numeroManifesto = int.Parse(nomeArquivo.Replace(PastasXml.Default.PastaManifestos + "\\", "").Replace(".xml", "")),
                    VolumesManifesto = (int)double.Parse(result[result.Count - 2].InnerText.Replace('.', ',')),
                    pesoManifesto = double.Parse(result[result.Count - 3].InnerText.Replace('.', ',')),
                    quantCtesManifesto = (int)double.Parse(result[result.Count - 4].InnerText.Replace('.', ','))
                };

                bool cadCte = docBD.cadastrarManifesto(lido);

                int cte;
                string fornecedor;

                for (int i = 0; i < result.Count - 4; i = i + 10)
                {
                    cte = int.Parse(result[i].InnerText);
                    CriarCte(cte, result[i + 1].InnerText);     //  alterado para novo cte

                    CriarCteManifesto(cte, lido.numeroManifesto);
                    fornecedor = result[i + 2].InnerText;
                    AlterarNfs(result[i + 1].InnerText, cte);
                }

                return true;
            }
            catch (System.Exception ex)
            {
                var olho = ex;
                return false;
            }
        }

        /// <summary>
        /// Importa os dados de xml de manifestos na pasta padrão
        /// </summary>
        public bool LerPreManifesto(string nomeArquivo)
        {
            try
            {
                XmlDocument manifesto = new XmlDocument();
                manifesto.Load(nomeArquivo);

                var ValueResult = manifesto.GetElementsByTagName("Value");
                var TextResult = manifesto.GetElementsByTagName("TextValue");
                DocumentosBD docBD = new DocumentosBD();

                var ctesNoXml = new List<string>();
                for (int i = 5; i < ValueResult.Count - 4; i = i + 6)
                { ctesNoXml.Add(ValueResult[i].InnerText); }
                var quantCtes = ctesNoXml.Distinct().Count();

                Manifestos lido = new Manifestos()
                {
                    numeroManifesto = int.Parse(nomeArquivo.Replace(PastasXml.Default.PastaPreManifestos + "\\", "").Replace(".xml", "")),
                    VolumesManifesto = (int)double.Parse(ValueResult[ValueResult.Count - 4].InnerText.Replace('.', ',')),
                    pesoManifesto = double.Parse(ValueResult[ValueResult.Count - 2].InnerText.Replace('.', ',')),
                    quantCtesManifesto = quantCtes
                };

                docBD.cadastrarManifesto(lido);

                int cte;
                int indexNF = 0;
                string fornecedor;
                List<Cte> ctesNoPreManifesto = new List<Cte>();
                for (int i = 5; i < ValueResult.Count - 4; i = i + 6)
                {
                    cte = int.Parse(ValueResult[i].InnerText.Replace('/', ' '));

                    Cte cteDaVez = ctesNoPreManifesto.Where(x => x.numeroCte == cte).Select(x => x).FirstOrDefault();
                    if (cteDaVez == null)
                    {
                        ctesNoPreManifesto.Add(new Cte(cte, TextResult[indexNF].InnerText.TrimStart('0')));
                    }
                    else
                        cteDaVez.notasCte += "\\" + TextResult[indexNF].InnerText.TrimStart('0');

                    fornecedor = ValueResult[i - 5].InnerText;
                    indexNF++;
                }
                foreach (var item in ctesNoPreManifesto)
                {
                    CriarCte(item.numeroCte, item.notasCte);    //  alterado para novo cte
                    AlterarNfs(item.notasCte, item.numeroCte);  //  alterado para novo cte
                    CriarCteManifesto(item.numeroCte, lido.numeroManifesto);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Importa os dados de xml de notas fiscais na pasta padrão
        /// </summary>
        public bool LerXmlNotaFiscal(string nomeArquivo)
        {
            try
            {
                NotasFiscais nfLida = new NotasFiscais();

                XmlDocument nf = new XmlDocument();

                nf.Load(nomeArquivo);

                var result = nf.GetElementsByTagName("det");
                nfLida.skuNF = result.Count;
                nfLida.numeroNF = nf.GetElementsByTagName("nNF")[0].InnerText;
                nfLida.numeroNF += '-' + nf.GetElementsByTagName("serie")[0].InnerText;

                var fornecedor = nf.GetElementsByTagName("xNome")[0].InnerText;
                if (fornecedor.Length > 49)
                    fornecedor = fornecedor.Remove(49);
                nfLida.fornecedorNF = fornecedor;
                try
                {
                    nfLida.volumesNF = int.Parse(nf.GetElementsByTagName("qVol")[0].InnerText);
                }
                catch
                {
                    var quantidade = nf.GetElementsByTagName("qCom");
                    nfLida.volumesNF = 0;
                    foreach (XmlElement item in quantidade)
                    {
                        nfLida.volumesNF += (int)double.Parse(item.InnerText.Replace('.', ','));
                    }
                }
                if (nfLida.volumesNF <= 1 && !nfLida.fornecedorNF.StartsWith("REGINA") && !nfLida.fornecedorNF.StartsWith("IMPROCROP"))
                {
                    var quantidade = nf.GetElementsByTagName("qCom");
                    nfLida.volumesNF = 0;
                    foreach (XmlElement item in quantidade)
                    {
                        nfLida.volumesNF += (int)double.Parse(item.InnerText.Replace('.', ','));
                    }
                }

                var cliente = nf.GetElementsByTagName("xNome")[1].InnerText;
                if (cliente.Length > 49)
                    cliente = cliente.Remove(49);
                nfLida.clienteNF = cliente;

                if (nfLida.skuNF > nfLida.volumesNF)
                { nfLida.skuNF = 1; }

                return InserirNotaFiscal(nfLida);
            }
            catch (System.Exception ex)
            {
                var erro = ex;
                return false;
            }
        }

        /// <summary>
        /// Importa os dados de notfis de notas fiscais na pasta padrão
        /// </summary>
        public bool LerNotfisNotaFiscal(string nomeArquivo)
        {
            try
            {
                NotasFiscais nfLida = new NotasFiscais();

                bool retorno = true;
                string[] notfis = File.ReadAllLines(nomeArquivo);
                string Fornecedor = "";

                if (notfis[0].StartsWith("000"))
                {
                    List<string> conteudoNf;
                    foreach (string nf in notfis)
                    {   //Fornecedor
                        if (nf.StartsWith("311"))
                        {
                            Fornecedor = nf.Remove(0, 133).TrimEnd(' ');
                        }

                        //Cliente e Zerar SKU
                        if (nf.StartsWith("312"))
                        {
                            nfLida.skuNF = 0;
                            conteudoNf = nf.Split(' ').ToList();
                            conteudoNf.RemoveAll(l => l == "");

                            nfLida.clienteNF = conteudoNf[0].Substring(3);

                            for (int i = 1; i < conteudoNf.Count; i++)
                            {
                                if (conteudoNf[i].Length >= 21)
                                    break;
                                else
                                    nfLida.clienteNF += " " + conteudoNf[i];
                            }
                            if (nfLida.clienteNF.Length > 49)
                                nfLida.clienteNF = nfLida.clienteNF.Remove(49);
                        }

                        //Volumes e número da NF
                        if (nf.StartsWith("313"))
                        {
                            conteudoNf = nf.Split(' ').ToList();
                            conteudoNf.RemoveAll(l => l == "");

                            nfLida.volumesNF = int.Parse(conteudoNf[conteudoNf.Count - 3].Remove(5));

                            string aux = conteudoNf[conteudoNf.Count - 1];

                            if (aux[0] == '3')
                                aux = aux.Substring(22, 12);
                            else
                                aux = aux.Substring(29, 12);

                            nfLida.numeroNF = aux.Remove(0, 3) + "-" + aux.Remove(3).TrimStart('0');
                            nfLida.numeroNF = nfLida.numeroNF.TrimStart('0');
                        }

                        //Contar SKUs
                        if (nf.StartsWith("314"))
                        {
                            conteudoNf = nf.Split(' ').ToList();
                            conteudoNf.RemoveAll(l => l == "");
                            nfLida.skuNF += conteudoNf.Count / 2;
                        }

                        //Salvar NF no banco de dados
                        if (nf.StartsWith("317"))
                        {
                            nfLida.fornecedorNF = Fornecedor;
                            retorno = InserirNotaFiscal(nfLida);
                        }
                    }
                }
                else
                {
                    retorno = false;
                }

                return retorno;
            }
            catch (System.Exception ex)
            {
                var erro = ex;
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private static bool InserirNotaFiscal(NotasFiscais nfLida)
        {
            DocumentosBD dbd = new DocumentosBD();
            if (dbd.verificarDocumentoCadastrado(2, nfLida.numeroNF) == 0)
            {
                if (!dbd.cadastrarNF(nfLida))
                { return false; }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Dispara a alteração das nfs de um manifesto para incluir o cte
        /// </summary>
        /// <param name="nfs">string contendo as notas fiscais separadas por uma '\'</param>
        private bool AlterarNfs(string nfs, int cte)
        {
            bool retorno = true;
            DocumentosBD dbd = new DocumentosBD();
            var listNfs = nfs.Split('\\');
            foreach (string nf in listNfs)
            {
                if (dbd.verificarDocumentoCadastrado(2, nf) >= 0)
                {
                    if (!dbd.inserirCteNf(nf, cte))
                        retorno = false; //false se não for possivel alterar alguma nota
                }
                else
                    retorno = false; //false se alguma nota não estiver cadastrada
            }
            return retorno;
        }

        private bool CriarCte(int cte, string notas)
        {
            DocumentosBD dbd = new DocumentosBD();
            //  alterado para novo cte
            return (dbd.cadastrarNovoCte(new Cte(cte, notas)));
        }

        private void CriarCteManifesto(int cte, int manifesto) // alterado para novo cte
        {
            DocumentosBD dbd = new DocumentosBD();
            var ctes = dbd.getNovoCtePorNum(cte);

            if (dbd.checarCteManifesto(new Cte_Manifesto(manifesto, ctes.Max(x => x.idCte))))
                dbd.cadastrarCteManifesto(new Cte_Manifesto(manifesto, ctes.Max(x => x.idCte)));
        }
    }
}