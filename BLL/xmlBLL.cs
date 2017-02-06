using DAL;
using DAL.Properties;
using System.Collections.Generic;
using System.IO;

namespace BLL
{
    public class xmlBLL
    {
        public void lerXML()
        {
            Xml xml = new Xml();
            List<string> arquivosPastaNF = new List<string>();
    
            foreach (string f in Directory.GetFiles(PastasXml.Default.PastaNFs))
            {
                var ext = extensao(f);
                if (ext == "msg" || ext == "MSG")
                {
                    xml.abrirEmail(moverArquivo(f));
                }
                else if (ext != "xml" && ext!="XML")
                    File.Delete(f);
            }
            foreach (string f in Directory.GetFiles(PastasXml.Default.PastaNFs))
            {
                var ext = extensao(f);
                if (ext == "xml" || ext == "XML")
                {
                    xml.lerNotaFiscal(f);
                    moverArquivo(f);
                }
                else
                    File.Delete(f);
            }

            foreach (string f in Directory.GetFiles(PastasXml.Default.PastaManifestos))
            {
                var ext = extensao(f);
                if (ext == "xml")
                {
                    xml.lerManifesto(f);
                    moverArquivo(f);
                }
                else
                    File.Delete(f);
            }
        }

        private string extensao(string caminho)
        {
            return string.Concat(caminho[caminho.Length - 3], caminho[caminho.Length - 2], caminho[caminho.Length - 1]);
        }

        private string moverArquivo(string nomeArquivo)
        {
            Directory.CreateDirectory(PastasXml.Default.PastaNFs + "\\old");
            Directory.CreateDirectory(PastasXml.Default.PastaManifestos + "\\old");
            string novaPasta;
            if (nomeArquivo.Contains(PastasXml.Default.PastaManifestos))
                novaPasta = nomeArquivo.Replace(PastasXml.Default.PastaManifestos, PastasXml.Default.PastaManifestos + "\\old");
            else
                novaPasta = nomeArquivo.Replace(PastasXml.Default.PastaNFs, PastasXml.Default.PastaNFs + "\\old");
            try { File.Move(nomeArquivo, novaPasta); }
            catch
            {
                File.Delete(nomeArquivo);
            }
            return novaPasta;
        }
    }
}