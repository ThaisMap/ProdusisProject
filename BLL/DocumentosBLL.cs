using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProdusisBD;
using DAL;

namespace BLL
{
    public class DocumentosBLL
    {
        DocumentosBD d = new DocumentosBD();
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

        public string fornecedorCte (int num)
        {
            return d.getFornecedorCte(num);
        }

       
        public double pesoCte(int num)
        {
            return d.getPesoCte(num);
        }

        public bool cteCadastrado(int num)
        {
            if (d.verificarDocumentoCadastrado(1, num.ToString()) != null)
            {
                return true;
            }
            else return false;
        }

        public NotasFiscais getDadosNF (string numero)
        {
            return d.getNFPorNumero(numero);
        }
    }
}
