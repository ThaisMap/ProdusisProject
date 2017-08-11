using DAL;
using DAL.Properties;
using ProdusisBD;
using System.Collections.Generic;

namespace BLL
{
    public class DocumentosBLL
    {
        private DocumentosBD d = new DocumentosBD();

        public Manifestos getDadosManifesto(int numero)
        {
            return d.getManifestoPorNumero(numero);
        }

        public string linhaDadosCte(int numero)
        {
            return d.getDadosDocumentos(1, numero);
        }

        public string linhaDadosManifesto(int numero)
        {
            return d.getDadosDocumentos(0, numero);
        }

        public int skuCte(int num)
        {
            return d.getSkuCte(num);
        }

        public int volumesCte(int num)
        {
            return d.getVolumesCte(num);
        }

        public int NFsImportadasNoManifesto(int numeroManifesto)
        {
            List<Cte_Manifesto> ctes = d.getCtesNoManifesto(numeroManifesto);
            int cont = 0;
            if(ctes != null)
            foreach (var item in ctes)
            {
                if (d.getSkuCte(item.Cte) > 0)
                    cont++;
            }
            return cont;
        }

        public string getFornecedorManifesto(int numManifesto)
        {
            return d.getFornecedorManifesto(numManifesto);
        }

        public string getNFsCte(int cte)
        {
            return d.getNfsCte(cte);
        }

        public string getManifestosCte(int cte)
        {
            return d.get_ListaManifestosCte(cte);
        }
            
        public string fornecedorCte(int num)
        {            
            return d.getFornecedorCte(num);
        }
        
        public bool cteCadastrado(int num)
        {
            if (d.verificarDocumentoCadastrado(1, num.ToString()) != 0)
            {
                return true;
            }
            else return false;
        }

        public bool manifestoCadastrado(int num)
        {
            if (d.verificarDocumentoCadastrado(0, num.ToString()) != 0)
            {
                return true;
            }
            else return false;
        }

        public NotasFiscais getDadosNF(string numero)
        {
            return d.getNFPorNumero(numero);
        }

        public string getPastaNFs()
        {
            return PastasXml.Default.PastaNFs;
        }

        public string getPastaPreManifestos()
        {
            return PastasXml.Default.PastaPreManifestos;
        }

        public string getPastaManifestos()
        {
            return PastasXml.Default.PastaManifestos;
        }

        public void setPastasNF(string caminho)
        {
            PastasXml.Default.PastaNFs = caminho;
            PastasXml.Default.Save();
        }

        public void setPastasManifesto(string caminho)
        {
            PastasXml.Default.PastaManifestos = caminho;
            PastasXml.Default.Save();
        }
        public void setPastasPreManifesto(string caminho)
        {
            PastasXml.Default.PastaPreManifestos = caminho;
            PastasXml.Default.Save();
        }
    }
}