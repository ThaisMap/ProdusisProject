using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdusisBD
{
    class TarefasBD
    {
        /// <summary>
        /// Insere um novo registro de tarefa no banco de dados
        /// </summary>
        /// <param name="novaTarefa">Dados do novo registro</param>
        /// <returns>True se o comando foi executado sem erros, False se houve algum erro</returns>
        public bool cadastrar(Tarefas novaTarefa)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.Tarefas.Add(novaTarefa);
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Edita o documento da tarefa indicada
        /// </summary>
        /// <returns>True se o comando foi executado sem erros, False se houve algum erro</returns>
        public bool editarDocumentoTarefa(int idTarefa, int novoDocumento)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Tarefas tarefaAtual = BancoDeDados.Tarefas.Single(f => f.idTarefa == idTarefa);
                    tarefaAtual.documentoTarefa = novoDocumento;
                   
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Grava a data/hora atuais como data/hora de fim da tarefa e desocupa os funcionários
        /// </summary>
        /// <returns>True se o comando foi executado sem erros, False se houve algum erro</returns>
        public bool finalizarTarefa(int idTarefa)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Tarefas tarefaAtual = BancoDeDados.Tarefas.Single(f => f.idTarefa == idTarefa);
                    tarefaAtual.fimTarefa = DateTime.Now;
                    BancoDeDados.SaveChanges();
                    FuncionariosBD func = new FuncionariosBD();
                    foreach (Func_Tarefa f in tarefaAtual.Func_Tarefa)
                    {
                        func.editarOcupacaoFuncionario(f.Funcionario, false);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Tarefas> getTarefasPendentes(int tipoTarefa)
        {
            try
            {

            }
            catch 
            {
                return new List<Tarefas>();
            }
            return new List<Tarefas>();
        }

        public List<Tarefas> getTarefasFiltradas(Filtros f)
        {
            return new List<Tarefas>();
        }
    }
}
