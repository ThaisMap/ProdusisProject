using DAL;
using DAL.Properties;
using ProdusisBD;
using System.Collections.Generic;

namespace BLL
{
    public class DocumentosBLL
    {
        private AcessoBD abd = new AcessoBD();
          

        /// <summary>
        /// Retorna o id do ultimo cte sem tarefa cadastrada com o número informado,
        /// ou 0 caso não haja disponível, -1 caso nao tenha nada importado
        /// </summary>
        /// <param name="numCte"></param>
        /// <returns></returns>
        public int getIdCteDisponivel(int numCte)
        {
            var lista = abd.GetNovoCtePorNum(numCte);
            if (lista.Count == 0)
                return -1;
            for (int i = lista.Count - 1; i >= 0; i--)
            {
                if (abd.VerificaDocumentoTarefa(lista[i].idCte, "2"))
                    return lista[i].idCte;
            }
            return 0;
        }

        public bool cteCadastrado(int num)
        {
            var idCtes = abd.GetNovoCtePorNum(num);
            if (idCtes.Count != 0)
            {
                return true;
            }
            else return false;
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