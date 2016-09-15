using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdusisBD
{
    class TarefasBD
    {
        public bool inserirTarefa(Tarefas novaTarefa)
        {
            return true;
        }

        public bool editarTarefa(Tarefas novaTarefa)
        {
            return true;
        }

        public List<Tarefas> getTarefasPendentes(int tipoTarefa)
        {
            return new List<Tarefas>();
        }

        public List<Tarefas> getTarefasFiltradas(Filtros f)
        {
            return new List<Tarefas>();
        }
    }
}
