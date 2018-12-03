using DAL;
using ProdusisBD;
using System.Collections.Generic;

namespace BLL
{
    public class Logica
    {
        private AcessoBD abd = new AcessoBD();
          

        public int IdCteDisponivelMaisRecente(int numCte)
        {
            var lista = abd.GetNovoCtePorNum(numCte);
            if (lista.Count == 0)
                return -1;
            for (int i = lista.Count - 1; i >= 0; i--)
            {
                if (!abd.VerificaDocumentoTarefa(lista[i].idCte, "2"))
                    return lista[i].idCte;
            }
            return 0;
        }

        public bool NumeroCteExiste(int num)
        {
            var idCtes = abd.GetNovoCtePorNum(num);
            if (idCtes.Count != 0)
            {
                return true;
            }
            else return false;
        }

        public bool CadastraObservacao(string nomeFunc, System.DateTime data, string texto)
        {
            int idFuncionario = abd.GetFuncPorNome(nomeFunc).idFunc;
            Observacoes obs = new Observacoes
            {
                FuncObs = idFuncionario,
                DataObs = data,
                TextoObs = texto
            };
            return abd.CadastrarObservacao(obs);
        }

        public bool InserirTarefa(Tarefas novaTarefa, string[] funcionarios)
        {
            if (novaTarefa.tipoTarefa != "2" && !abd.ManifestoExiste(novaTarefa.documentoTarefa))
            {
                return false;
            }

            int[] idsFuncionarios = new int[funcionarios.Length];

            novaTarefa.divergenciaTarefa = "-;0;-;0;-;0";
            for (int i = 0; i < funcionarios.Length; i++)
            {
                idsFuncionarios[i] = abd.GetFuncPorNome(funcionarios[i]).idFunc;
            }
            return abd.CadastrarTarefa(novaTarefa, idsFuncionarios);
        }
    }
}