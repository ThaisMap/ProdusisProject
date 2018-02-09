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
        public void extrairAnexosDeEmail(string nomeArquivo, Application Outlook)
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
        public bool lerManifesto(string nomeArquivo)
        {
            try
            {
                Manifestos lido = new Manifestos();

                XmlDocument manifesto = new XmlDocument();
                manifesto.Load(nomeArquivo);

                DocumentosBD docBD = new DocumentosBD();
                lido.numeroManifesto = int.Parse(nomeArquivo.Replace(PastasXml.Default.PastaManifestos + "\\", "").Replace(".xml", ""));

                var result = manifesto.GetElementsByTagName("Value");
                lido.VolumesManifesto = (int)double.Parse(result[result.Count - 2].InnerText.Replace('.', ','));
                lido.pesoManifesto = double.Parse(result[result.Count - 3].InnerText.Replace('.', ','));
                lido.quantCtesManifesto = (int)double.Parse(result[result.Count - 4].InnerText.Replace('.', ','));

                bool cadCte = docBD.cadastrarManifesto(lido);

                int cte;
                string fornecedor;

                for (int i = 0; i < result.Count - 4; i = i + 10)
                {
                    cte = int.Parse(result[i].InnerText);
                    criarCte(cte);

                    criarCteManifesto(cte, lido.numeroManifesto);
                    fornecedor = result[i + 2].InnerText;
                    alterarNfs(result[i + 1].InnerText, cte, fornecedor);
                }

                docBD.alterarSkuManifesto(lido.numeroManifesto);
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
        public bool lerPreManifesto(string nomeArquivo)
        {
            try
            {
                XmlDocument manifesto = new XmlDocument();
                manifesto.Load(nomeArquivo);

                var ValueResult = manifesto.GetElementsByTagName("Value");
                var TextResult = manifesto.GetElementsByTagName("TextValue");
                DocumentosBD docBD = new DocumentosBD();
                Manifestos lido = new Manifestos()
                {
                    numeroManifesto = int.Parse(nomeArquivo.Replace(PastasXml.Default.PastaPreManifestos + "\\", "").Replace(".xml", "")),
                    VolumesManifesto = (int)double.Parse(ValueResult[ValueResult.Count - 4].InnerText.Replace('.', ',')),
                    pesoManifesto = double.Parse(ValueResult[ValueResult.Count - 1].InnerText.Replace('.', ',')),
                    quantCtesManifesto = (int)double.Parse(ValueResult[ValueResult.Count - 2].InnerText.Replace('.', ','))
                };

                bool cadCte = docBD.cadastrarManifesto(lido);

                int cte;
                int indexNF = 0;
                int skuTotal = 0;
                string fornecedor;

                for (int i = 2; i < ValueResult.Count - 4; i = i + 6)
                {
                    cte = int.Parse(ValueResult[i].InnerText);
                    criarCte(cte);
                    if (cadCte)
                        criarCteManifesto(cte, lido.numeroManifesto);

                    fornecedor = ValueResult[i + 2].InnerText;
                    alterarUmaNf(TextResult[indexNF].InnerText, cte, fornecedor);
                    skuTotal += docBD.getSkuCte(cte);
                    indexNF++;
                }

                docBD.alterarSkuManifesto(lido.numeroManifesto, skuTotal);
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
        public bool lerXmlNotaFiscal(string nomeArquivo)
        {
            try
            {
                NotasFiscais nfLida = new NotasFiscais();

                XmlDocument nf = new XmlDocument();

                nf.Load(nomeArquivo);

                var result = nf.GetElementsByTagName("det");
                nfLida.skuNF = result.Count;
                nfLida.numeroNF = nf.GetElementsByTagName("nNF")[0].InnerText + '-' + nf.GetElementsByTagName("serie")[0].InnerText;

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
                if (nfLida.volumesNF <= 1)
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

                return inserirNotaFiscal(nfLida);
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
        public bool lerNotfisNotaFiscal(string nomeArquivo)
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
                        if (nf.StartsWith("000"))
                        {
                            conteudoNf = nf.Split(' ').ToList();
                            conteudoNf.RemoveAll(l => l == "");

                            Fornecedor = conteudoNf[0].Remove(0, 3);
                            for (int i = 1; i < conteudoNf.Count - 2; i++)
                            {
                                Fornecedor += " ";
                                Fornecedor += conteudoNf[i];
                            }
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
                            retorno = inserirNotaFiscal(nfLida);
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

        private static bool inserirNotaFiscal(NotasFiscais nfLida)
        {
            DocumentosBD dbd = new DocumentosBD();
            if (!dbd.cadastrarNF(nfLida))
            { return false; }

            return true;
        }

        /// <summary>
        /// Dispara a alteração das nfs de um manifesto para incluir o cte
        /// </summary>
        /// <param name="nfs">string contendo as notas fiscais separadas por uma '\'</param>
        private bool alterarNfs(string nfs, int cte, string fornecedor)
        {
            bool retorno = true;
            DocumentosBD dbd = new DocumentosBD();
            var listNfs = nfs.Split('\\');
            foreach (string nf in listNfs)
            {
                if (dbd.verificarDocumentoCadastrado(2, nf) >= 0)
                {
                    if (!dbd.inserirCteNf(nf, cte, fornecedor))
                        retorno = false; //false se não for possivel alterar alguma nota
                }
                else
                    retorno = false; //false se alguma nota não estiver cadastrada
            }
            return retorno;
        }

        /// <summary>
        /// Dispara a alteração de uma nf para incluir o cte
        /// </summary>
        private bool alterarUmaNf(string nf, int cte, string fornecedor)
        {
            DocumentosBD dbd = new DocumentosBD();
            nf = nf.TrimStart('0');
            NotasFiscais nova = dbd.getNFPorNumero(nf);
            if (nova != null)
            {
                return dbd.inserirCteNf(nf, cte, fornecedor);
            }
            else
                return false; //false se a nota não estiver cadastrada
        }

        private bool criarCte(int cte)
        {
            DocumentosBD dbd = new DocumentosBD();
            return dbd.cadastrarCte(new Ctes(cte));
        }

        private void criarCteManifesto(int cte, int manifesto)
        {
            DocumentosBD dbd = new DocumentosBD();
            if (dbd.checarCteManifesto(new Cte_Manifesto(cte, manifesto)))
                dbd.cadastrarCteManifesto(new Cte_Manifesto(cte, manifesto));
        }
    }
}