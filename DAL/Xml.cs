using DAL.Properties;
using Microsoft.Office.Interop.Outlook;
using ProdusisBD;
using System.Xml;
using System;

namespace DAL
{
    public class Xml
    {
        public void abrirEmail(string nomeArquivo)
        {
            Application app = new Application();
            try
            {
                MailItem email = (MailItem)app.Session.OpenSharedItem(nomeArquivo);

                foreach (Attachment anexo in email.Attachments)
                {
                        string nomeXML = PastasXml.Default.PastaNFs + "\\" + anexo.FileName;
                        anexo.SaveAsFile(nomeXML);                    
                }

                app.Quit();
                email.Close(OlInspectorClose.olDiscard);
            }
            catch 
            {
                try
                {
                    app.Quit();
                }
                catch { }
            }

        }

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

                if (!docBD.cadastrarManifesto(lido))
                { return false; }

                int cte;
                for (int i = 0; i < result.Count - 4; i = i + 10)
                {
                    cte = int.Parse(result[i].InnerText);
                    criarCte(cte);
                    alterarNfs(result[i + 1].InnerText, result[i + 7].InnerText, cte);
                    criarCteManifesto(cte, lido.numeroManifesto);
                }

                docBD.alterarSkuManifesto(lido.numeroManifesto);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool lerNotaFiscal(string nomeArquivo)
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
                nfLida.fonecedorNF = fornecedor;
                nfLida.volumesNF = int.Parse(nf.GetElementsByTagName("qVol")[0].InnerText);
                nfLida.pesoNF = double.Parse(nf.GetElementsByTagName("pesoB")[0].InnerText.Replace(".", ","));

                DocumentosBD dbd = new DocumentosBD();
                if (!dbd.cadastrarNF(nfLida))
                { return false; }

                return true;
            }
            catch (System.Exception ex)
            {
                var erro = ex;
                return false;
            }
        }

        private bool alterarNfs(string nfs, string fornecedor, int cte)
        {
            bool retorno = true;
            DocumentosBD dbd = new DocumentosBD();
            var listNfs = nfs.Split('\\');
            foreach (string nf in listNfs)
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
            Cte_Manifesto novo = new Cte_Manifesto();
            novo.Cte = cte;
            novo.Manifesto = manifesto;
            dbd.cadastrarCteManifesto(novo);
        }
  }
}