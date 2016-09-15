using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdusisBD
{
    class DocumentosBD
    {
        public bool inserirManifesto(Manifestos novoManifesto)
        {
            return true;
        }

        public bool inserirCte(Ctes novoCte)
        {
            return true;
        }

        public bool inserirNF(Ctes novaNf)
        {
            return true;
        }

        public bool inserirCteManifesto(int idCte, int idManifesto)
        {
            return true;
        }

        public Manifestos getManifestoPorNumero(int numManifesto)
        {
            return new Manifestos();
        }

        public NotasFiscais getNFPorNumero(int numNF)
        {
            return new NotasFiscais();
        }

        public Ctes getCtePorNumero(int numCte)
        {
            return new Ctes();
        }

        public bool verificarDocumentoCadastrado(int numDocumento)
        {
            return true;
        }

        public string getDadosDocumentos(int tipoDocumento, int numDocumento)
        {
            return "dados do documento";
        }

        public bool verificaNotasManifesto(int numManifesto)
        {
            return true;
        }

        public int getSkuManifesto(int numManifesto)
        {
            return 1;
        }

        public int getSkuCte(int numCte)
        {
            return 1;
        }

        public double getPesoManifesto(int numCte)
        {
            return 1;
        }

        public int getVolumesManifesto(int numCte)
        {
            return 1;
        }

        private int getSkuNF(int numNF)
        {
            return 1;
        }

        private double getPesoNF(int numNF)
        {
            return 1;
        }

        private int getVolumesNF(int numNF)
        {
            return 1;
        }

    }
}
