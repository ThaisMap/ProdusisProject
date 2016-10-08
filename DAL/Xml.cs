using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProdusisBD;
using DAL.Properties;
using System.Xml;
using Microsoft.Office.Interop.Outlook;

namespace DAL
{
    public class Xml
    {
        public bool lerNotaFiscal(string nomeArquivo)
        {
            try
            {
                if (isXml(nomeArquivo))
                {
                    NotasFiscais nfLida = new NotasFiscais();

                    XmlDocument nf = new XmlDocument();

                    nf.Load(nomeArquivo);

                    var result = nf.GetElementsByTagName("det");
                    nfLida.skuNF = result.Count;
                    nfLida.numeroNF = nf.GetElementsByTagName("nNF")[0].InnerText + '-' + nf.GetElementsByTagName("serie")[0].InnerText;
                    nfLida.fonecedorNF = nf.GetElementsByTagName("xNome")[0].InnerText;
                    nfLida.destinatarioNF = nf.GetElementsByTagName("xNome")[1].InnerText;
                    nfLida.volumesNF = int.Parse(nf.GetElementsByTagName("qVol")[0].InnerText);
                    nfLida.pesoNF = double.Parse(nf.GetElementsByTagName("pesoB")[0].InnerText.Replace(".", ","));

                    DocumentosBD dbd = new DocumentosBD();
                    if (!dbd.cadastrarNF(nfLida)) { return false; }

                    return true;
                }
                return false;
            }

            catch
            {
                return false;
            }
        }

        public bool lerManifesto(string nomeArquivo)
        {
            try
            {
                if (isXml(nomeArquivo))
                {
                    Manifestos lido = new Manifestos();

                    XmlDocument manifesto = new XmlDocument();
                    manifesto.Load(nomeArquivo);

                    DocumentosBD dbd = new DocumentosBD();
                    lido.numeroManifesto = int.Parse(nomeArquivo.Replace(PastasXml.Default.PastaManifestos + "\\", "").Replace(".xml", ""));

                    var result = manifesto.GetElementsByTagName("Value");
                    lido.VolumesManifesto = (int)double.Parse(result[result.Count - 2].InnerText.Replace('.', ','));
                    lido.pesoManifesto = double.Parse(result[result.Count - 3].InnerText.Replace('.', ','));
                    lido.quantCtesManifesto = (int)double.Parse(result[result.Count - 4].InnerText.Replace('.', ','));

                    if (!dbd.cadastrarManifesto(lido)) { return false; }

                    int cte;
                    for (int i = 0; i < result.Count - 4; i = i + 10)
                    {
                        cte = int.Parse(result[i].InnerText);
                        criarCte(cte);
                        alterarNfs(result[i + 1].InnerText, result[i + 7].InnerText, cte);
                        criarCteManifesto(cte, lido.numeroManifesto);
                    }
                    return true;
                }
                return false;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }

        private void criarCte(int cte)
        {
            DocumentosBD dbd = new DocumentosBD();
            Ctes novoCte = new Ctes();
            novoCte.numeroCte = cte;
            dbd.cadastrarCte(novoCte);            
        }

        private void criarCteManifesto(int cte, int manifesto)
        {
            DocumentosBD dbd = new DocumentosBD();
            cte = dbd.getCtePorNumero(cte).idCte;
            manifesto = dbd.getManifestoPorNumero(manifesto).idManifesto;
            Cte_Manifesto novo = new Cte_Manifesto();
            novo.Cte = cte;
            novo.Manifesto = manifesto;
            dbd.cadastrarCteManifesto(novo);
        }

        private bool alterarNfs(string nfs, string fornecedor, int cte)
        {
            bool retorno = true;
            DocumentosBD dbd = new DocumentosBD();
            var listNfs = nfs.Split('\\');
            foreach(string nf in listNfs)
            {
                if (dbd.verificarDocumentoCadastrado(2, nf) >= 0)
                {
                    if (!dbd.inserirCteNf(nf, fornecedor, cte))
                        retorno = false; //false se não for possivel alterar alguma nota
                }
                else
                    retorno = false; //false se alguma nota não estiver cadastrada
             
            }
            return retorno;
        }
   
        public void abrirEmail(string nomeArquivo)
        {
            Application app = new Application();
            MailItem email = (MailItem)app.Session.OpenSharedItem(nomeArquivo);

            foreach (Attachment anexo in email.Attachments)
            {
                if (isXml(anexo.FileName))
                {
                    string nomeXML = PastasXml.Default.PastaNFs + "\\" + anexo.FileName;
                    anexo.SaveAsFile(nomeXML);
                }
            }

            app.Quit();
        }
            
        private bool isXml(string caminho)
        {
            string extensao = String.Concat(caminho[caminho.Length - 3], caminho[caminho.Length - 2], caminho[caminho.Length - 1]);
            if (extensao == "xml") return true;
            return false;
        }
    }
}
