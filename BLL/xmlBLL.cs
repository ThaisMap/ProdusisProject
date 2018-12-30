using DAL;
using DAL.Properties;
using Microsoft.Office.Interop.Outlook;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BLL
{
    public class xmlBLL
    {
        private Application Outlook;

        /// <summary>
        /// Processa arquivos nas pastas de nf e manifesto
        /// </summary>
        public void TriagemArquivos()
        {
            try
            {
                Xml xml = new Xml();
                List<string> arquivosPastaNF = new List<string>();
                
                //Percorre pasta de manifestos, importa os dados do xml e apaga o restante
                foreach (string f in Directory.GetFiles(PastasXml.Default.PastaManifestos))
                {
                    if (f.EndsWith("xml") || f.EndsWith("XML"))
                    {                        
                        xml.LerManifesto(f);
                        MoverArquivo(f);
                    }
                    //else
                       // File.Delete(f);
                }
          
                var listaDeArquivos = Directory.GetFiles(PastasXml.Default.PastaNFs);
                bool temMSG = listaDeArquivos.Where(f => f.EndsWith("msg") || f.EndsWith("MSG")).Any();
                

                //Percorre pasta de NFs pela primeira vez, ao encontrar um email , extrai os anexos e mover o email para pasta old
                if (temMSG)
                {
                    Outlook = new Application();
                    foreach (string f in listaDeArquivos)
                    {
                        if (f.EndsWith("msg") || f.EndsWith("MSG"))
                        {
                            xml.ExtrairAnexosDeEmail(MoverArquivo(f), Outlook);
                        }
                    }
                    Outlook.Quit();
                }

                //Percorre pasta de NFs pela segunda vez
                //Ao encontrar xml ou txt, procede com importação dos dados e move o arquivo para pasta old
                //Qualquer outro tipo de arquivo é excluido
                foreach (string f in Directory.GetFiles(PastasXml.Default.PastaNFs))
                {
                    if (f.EndsWith("xml") || f.EndsWith("XML"))
                    {
                        xml.LerXmlNotaFiscal(f);
                        MoverArquivo(f);
                    }
                    else
                    if (f.EndsWith("txt") || f.EndsWith("TXT"))
                    {
                        xml.LerNotfisNotaFiscal(f);
                        MoverArquivo(f);
                    }
                    else
                        File.Delete(f);
                }
     
                //Percorre pasta de manifestos, importa os dados do xml e apaga o restante
                foreach (string f in Directory.GetFiles(PastasXml.Default.PastaPreManifestos))
                {
                    if (f.EndsWith("xml") || f.EndsWith("XML"))
                    {
                        xml.LerPreManifesto(f);
                        MoverArquivo(f);
                    }
                    else
                        File.Delete(f);
                }


               
            }
            catch (System.Exception)
            {
            }
        }       

        /// <summary>
        /// Move o arquivo da pasta padrão para a pasta old respectiva
        /// </summary>
        private string MoverArquivo(string nomeArquivo)
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