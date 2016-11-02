using DAL;
using ProdusisBD;
using System.Collections.Generic;

namespace BLL
{
    public class TarefasBLL
    {
        private TarefasBD t = new TarefasBD();

        public bool inserirTarefa(Tarefas novaTarefa, string[] funcionarios)
        {
            FuncionariosBD f = new FuncionariosBD();
            int[] idsFuncionarios = new int[funcionarios.Length];
            for (int i = 0; i < funcionarios.Length; i++)
            {
                idsFuncionarios[i] = f.getFuncPorNome(funcionarios[i]).idFunc;
            }
            return t.cadastrar(novaTarefa, idsFuncionarios);
        }

        public List<TarefaModelo> tarefasPendentes(string tipo)
        {
            return t.getTarefasPendentes(tipo);
        }

        public bool tarefaRepetida(int documento, string tipo)
        {
            return t.verificaDocumentoTarefa(documento, tipo);
        }

        public bool finalizarTarefa(int idTarefa)
        {
            return t.finalizarTarefa(idTarefa);
        }
    }
}