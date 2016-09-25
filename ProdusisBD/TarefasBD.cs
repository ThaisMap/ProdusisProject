using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Data.Entity;

namespace ProdusisBD
{
    public class TarefasBD
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

        /// <summary>
        /// Retorna uma lista com as tarefas do tipo indicado sem hora de finalização
        /// </summary>
        public List<Tarefas> getTarefasPendentes(string tipoTarefa)
        {
            List<Tarefas> pendentes = new List<Tarefas>();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    pendentes = (from Tarefas in BancoDeDados.Tarefas where Tarefas.fimTarefa == null where Tarefas.tipoTarefa == tipoTarefa select Tarefas).ToList();
                }
                return pendentes;
            }
            catch 
            {
                return new List<Tarefas>();
            }
        }

        /// <summary>
        /// REtorna uma lista com as tarefas que atendam os filtros informados
        /// </summary>
        /// <param name="f">Parâmetros de pesquisa</param>
        public List<Tarefas> getTarefasFiltradas(Filtros f)
        {
            List<Tarefas> pesquisados = new List<Tarefas>();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var query = BancoDeDados.Tarefas.AsQueryable();

                    query = query.Where(t => t.inicioTarefa >= f.dataInicio);
                    if (f.numDocumento > -1)
                        query = query.Where(t => t.documentoTarefa == f.numDocumento);


                    if (f.dataFim != null)
                        query = query.Where(t => t.fimTarefa <= f.dataFim);


                    if (f.TipoTarefa != "")
                        query = query.Where(t => t.tipoTarefa == f.TipoTarefa);


                    if (f.idFuncionario > -1)
                        query = from t in query
                                 join ft in BancoDeDados.Func_Tarefa on t.idTarefa equals ft.Tarefa
                                 where ft.Funcionario == f.idFuncionario
                                 select t;

                    if (f.dataFim != null)
                        query = query.Where(t => t.fimTarefa <= f.dataFim);

                    if (f.volumeInicio > 0)
                    {
                        var getManifestos = query.Where(t => t.Manifestos.VolumesManifesto >= f.volumeInicio);
                        var getCtes = from t in query
                                      where t.Ctes.NotasFiscais.Sum(NF => NF.volumesNF) >= f.volumeInicio
                                      select t;
                        query = getManifestos.Union(getCtes);
                    }

                    if (f.volumeFim > 0)
                    {
                        var getManifestos = query.Where(t => t.Manifestos.VolumesManifesto <= f.volumeFim);
                        var getCtes = from t in query
                                      where t.Ctes.NotasFiscais.Sum(NF => NF.volumesNF) <= f.volumeFim
                                      select t;
                        query = getManifestos.Union(getCtes);
                    }

                    if (f.skuInicio > 0)
                    {
                        var getManifestos = query.Where(t => t.Manifestos.skusManifesto >= f.skuInicio);
                        var getCtes = from t in query
                                      where t.Ctes.NotasFiscais.Sum(NF => NF.skuNF) >= f.skuInicio
                                      select t;
                        query = getManifestos.Union(getCtes);
                    }

                    if (f.skuFim > 0)
                    {
                        var getManifestos = query.Where(t => t.Manifestos.skusManifesto <= f.skuFim);
                        var getCtes = from t in query
                                      where t.Ctes.NotasFiscais.Sum(NF => NF.skuNF) <= f.skuFim
                                      select t;
                        query = getManifestos.Union(getCtes);
                    }

                    if (f.pesoInicio > 0)
                    {
                        var getManifestos = query.Where(t => t.Manifestos.pesoManifesto >= f.pesoInicio);
                        var getCtes = from t in query
                                      where t.Ctes.NotasFiscais.Sum(NF => NF.pesoNF) >= f.pesoInicio
                                      select t;
                        query = getManifestos.Union(getCtes);
                    }

                    if (f.pesoFim > 0)
                    {
                        var getManifestos = query.Where(t => t.Manifestos.pesoManifesto <= f.pesoFim);
                        var getCtes = from t in query
                                      where t.Ctes.NotasFiscais.Sum(NF => NF.pesoNF) <= f.pesoFim
                                      select t;
                        query = getManifestos.Union(getCtes);
                    }

                    return query.ToList();
                }
             
            }
            catch
            {
                return new List<Tarefas>();
            }
        }
    }
}
