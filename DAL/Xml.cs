using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProdusisBD;
using DAL.Properties;
using System.Xml;

namespace DAL
{
    public class Xml
    {
        public NotasFiscais lerNotaFiscal(string nomeArquivo)
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
            nfLida.pesoNF = double.Parse(nf.GetElementsByTagName("pesoB")[0].InnerText.Replace(".",","));

            return new NotasFiscais();
        }

        public Manifestos lerManifesto(string nomeArquivo)
        {try
            {
                Manifestos lido = new Manifestos();

                XmlDocument manifesto = new XmlDocument();
                manifesto.Load(nomeArquivo);

                DocumentosBD dbd = new DocumentosBD();
                lido.numeroManifesto = int.Parse(nomeArquivo.Replace(PastasXml.Default.PastaManifestos + "//", "").Replace(".xml", ""));
                int cte;

                var result = manifesto.GetElementsByTagName("Value");
                for (int i = 0; i < result.Count - 4; i = i + 10)
                {
                    cte = int.Parse(result[i].InnerText);
                    criarCte(cte);
                    criarCteManifesto(cte, lido.numeroManifesto);
                    alterarNfs(result[i + 1].InnerText, result[i + 7].InnerText, cte);
                }

                lido.VolumesManifesto = int.Parse(result[result.Count - 1].InnerText.Replace('.', ','));
                lido.pesoManifesto = double.Parse(result[result.Count - 2].InnerText.Replace('.', ','));
                lido.quantCtesManifesto = int.Parse(result[result.Count - 3].InnerText.Replace('.', ','));
                                 
                return lido;
            }
            catch (Exception e)
            {
                return new Manifestos();
            }
        }

        private void criarCte(int cte)
        {
            DocumentosBD dbd = new DocumentosBD();
            dbd.cadastrarCte(new Ctes(cte));            
        }

        private void criarCteManifesto(int cte, int manifesto)
        {
            DocumentosBD dbd = new DocumentosBD();
            dbd.cadastrarCteManifesto(new Cte_Manifesto(cte, manifesto));
        }

        private bool alterarNfs(string nfs, string fornecedor, int cte)
        {
            bool retorno = true;
            DocumentosBD dbd = new DocumentosBD();
            var listNfs = nfs.Split('\\');
            foreach(string nf in listNfs)
            {
                if (dbd.verificarDocumentoCadastrado(2, nf) >= 0)
                    if (!dbd.inserirCteNf(nf, fornecedor, cte))
                        retorno = false; //false se não for possivel alterar alguma nota
                    else
                        retorno = false; //false se alguma nota não estiver cadastrada
             
            }
            return retorno;
        }
       
    }
}
