using DAL;
using DAL.Properties;
using System.IO;

namespace BLL
{
    public class xmlBLL
    {
        public void lerXML()
        {
            Xml xml = new Xml();

            foreach (string f in Directory.GetFiles(PastasXml.Default.PastaNFs))
            {
                var ext = extensao(f);
                if (ext == "msg")
                    xml.abrirEmail(f);
                else if (ext == "xml")
                    xml.lerNotaFiscal(f);
                moverArquivo(f);
            }

            foreach (string f in Directory.GetFiles(PastasXml.Default.PastaManifestos))
            {
                var ext = extensao(f);
                if (ext == "xml")
                    xml.lerManifesto(f);
                moverArquivo(f);
            }
        }

        private string extensao(string caminho)
        {
            return string.Concat(caminho[caminho.Length - 3], caminho[caminho.Length - 2], caminho[caminho.Length - 1]);
        }

        private void moverArquivo(string nomeArquivo)
        {
            Directory.CreateDirectory(PastasXml.Default.PastaNFs + "\\old");
            Directory.CreateDirectory(PastasXml.Default.PastaManifestos + "\\old");
            string novaPasta;
            if (nomeArquivo.Contains(PastasXml.Default.PastaManifestos))
                novaPasta = nomeArquivo.Replace(PastasXml.Default.PastaManifestos, PastasXml.Default.PastaManifestos + "\\old");
            else
                novaPasta = nomeArquivo.Replace(PastasXml.Default.PastaNFs, PastasXml.Default.PastaNFs + "\\old");

            File.Move(nomeArquivo, novaPasta);
        }
    }
}