using DAL;
using DAL.Properties;
using ProdusisBD;

namespace BLL
{
    public class DocumentosBLL
    {
        private DocumentosBD d = new DocumentosBD();

        public Manifestos getDadosManifesto(int numero)
        {
            return d.getManifestoPorNumero(numero);
        }

        public string linhaDados(int numero)
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

        public string fornecedorCte(int num)
        {
            return d.getFornecedorCte(num);
        }

        public double pesoCte(int num)
        {
            return d.getPesoCte(num);
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
    }
}