using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProdusisBD;
using DAL;


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

        public List<Tarefas> tarefasPendentes (string tipo)
        {
            return t.getTarefasPendentes(tipo);
        }

        public bool finalizarTarefa(int idTarefa)
        {
            return t.finalizarTarefa(idTarefa);
        }
    }
}
