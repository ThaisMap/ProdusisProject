using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DAL;
using DAL.Properties;

namespace BLL
{
    public class xmlBLL
    {
        public void lerXML()
        {
            Xml xml = new Xml();                      
                     

            foreach (string f in Directory.GetFiles(PastasXml.Default.PastaNFs))
            {
                //xml.abrirEmail(f);
                xml.lerNotaFiscal(f);
            }

            foreach (string f in Directory.GetFiles(PastasXml.Default.PastaManifestos))
            {
                xml.lerManifesto(f);
            }
        }
    }
}
