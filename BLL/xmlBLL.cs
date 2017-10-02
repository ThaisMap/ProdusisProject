using DAL;
using DAL.Properties;
using Microsoft.Office.Interop.Outlook;
using System.Collections.Generic;
using System.IO;

namespace BLL
{
    public class xmlBLL
    {
        Application Outlook;
        /// <summary>
        /// Processa arquivos nas pastas de nf e manifesto
        /// </summary>
        public void triagemArquivos()
        {
            try
            {
                Xml xml = new Xml();
                List<string> arquivosPastaNF = new List<string>();
                
               var listaDeArquivos = Directory.GetFiles(PastasXml.Default.PastaNFs);
                bool temMSG = false;
                foreach (string f in listaDeArquivos)
                {
                    var ext = extensao(f);
                    if (ext == "msg" || ext == "MSG")
                    {
                        temMSG = true;

                        break;
                    }
                }

                //Percorre pasta de NFs pela primeira vez, ao encontrar um email , extrai os anexos e mover o email para pasta old
                if (temMSG)
                {
                    Outlook = new Application();
                    foreach (string f in listaDeArquivos)
                    {
                        var ext = extensao(f);
                        if (ext == "msg" || ext == "MSG")
                        {
                            xml.extrairAnexosDeEmail(moverArquivo(f), Outlook);
                        }
                    }
                    Outlook.Quit();
                }
                

                //Percorre pasta de NFs pela segunda vez
                //Ao encontrar xml ou txt, procede com importação dos dados e move o arquivo para pasta old
                //Qualquer outro tipo de arquivo é excluido
                foreach (string f in Directory.GetFiles(PastasXml.Default.PastaNFs))
                {
                    var ext = extensao(f);
                    if (ext == "xml" || ext == "XML")
                    {
                        xml.lerXmlNotaFiscal(f);
                        moverArquivo(f);
                    }
                    else
                        if (ext == "txt" || ext == "TXT")
                    {
                        xml.lerNotfisNotaFiscal(f);
                        moverArquivo(f);
                    }
                    else
                        File.Delete(f);
                }

                //Percorre pasta de manifestos, importa os dados do xml e apaga o restante
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
                //Percorre pasta de manifestos, importa os dados do xml e apaga o restante
                foreach (string f in Directory.GetFiles(PastasXml.Default.PastaPreManifestos))
                {
                    var ext = extensao(f);
                    if (ext == "xml")
                    {
                        xml.lerPreManifesto(f);
                        moverArquivo(f);
                    }
                    else
                        File.Delete(f);
                }
            }
            catch (System.Exception ex)
            {
                var erro = ex;
            }
        }

        /// <summary>
        /// Retorna ultimas 3 letras do arquivo informado
        /// </summary>
        private string extensao(string caminho)
        {
            return string.Concat(caminho[caminho.Length - 3], caminho[caminho.Length - 2], caminho[caminho.Length - 1]);
        }

        /// <summary>
        /// Move o arquivo da pasta padrão para a pasta old respectiva
        /// </summary>
        private string moverArquivo(string nomeArquivo)
        {
            Directory.CreateDirectory(PastasXml.Default.PastaNFs + "\\old");
            Directory.CreateDirectory(PastasXml.Default.PastaManifestos + "\\old");
            Directory.CreateDirectory(PastasXml.Default.PastaPreManifestos + "\\old");
            string novaPasta;
            if (nomeArquivo.Contains(PastasXml.Default.PastaManifestos))
                novaPasta = nomeArquivo.Replace(PastasXml.Default.PastaManifestos, PastasXml.Default.PastaManifestos + "\\old");
            else
                if (nomeArquivo.Contains(PastasXml.Default.PastaPreManifestos))
                novaPasta = nomeArquivo.Replace(PastasXml.Default.PastaPreManifestos, PastasXml.Default.PastaPreManifestos + "\\old");
            else
                novaPasta = nomeArquivo.Replace(PastasXml.Default.PastaNFs, PastasXml.Default.PastaNFs + "\\old");
            try { File.Move(nomeArquivo, novaPasta); }
            catch
            {
                File.Delete(novaPasta);
                File.Move(nomeArquivo, novaPasta);
            }
            return novaPasta;
        }
    }
}