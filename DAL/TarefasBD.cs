﻿using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public class TarefasBD
    {
        /// <summary>
        /// Insere um novo registro de tarefa no banco de dados
        /// </summary>
        /// <param name="novaTarefa">Dados do novo registro</param>
        /// <returns>True se o comando foi executado sem erros, False se houve algum erro</returns>
        public bool cadastrar(Tarefas novaTarefa, int[] funcionarios)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.Tarefas.Add(novaTarefa);
                    BancoDeDados.SaveChanges();
                    Func_Tarefa ft;
                    FuncionariosBD fBd = new FuncionariosBD();
                    foreach (int i in funcionarios)
                    {
                        ft = new Func_Tarefa();
                        ft.Tarefa = novaTarefa.idTarefa;
                        ft.Funcionario = i;
                        BancoDeDados.Func_Tarefa.Add(ft);
                        BancoDeDados.SaveChanges();
                        fBd.editarOcupacaoFuncionario(i, true);
                    }
                }
                return true;
            }
            catch (Exception e)
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
        public List<TarefaModelo> getTarefasPendentes(string tipoTarefa)
        {
            List<Tarefas> pendentes = new List<Tarefas>();
            List<TarefaModelo> pendentesModelo = new List<TarefaModelo>();
            FuncionariosBD f = new FuncionariosBD();
            TarefaModelo Aux;

            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    pendentes = (from Tarefas in BancoDeDados.Tarefas where Tarefas.fimTarefa == null where Tarefas.tipoTarefa == tipoTarefa select Tarefas).ToList();
                    foreach (Tarefas tarefa in pendentes)
                    {
                        Aux = new TarefaModelo(tarefa);

                        foreach (var func in tarefa.Func_Tarefa)
                        {
                            if (Aux.nomesFuncionarios != null)
                                Aux.nomesFuncionarios += "/" + f.getFuncPorId(func.Funcionario).nomeFunc;
                            else
                                Aux.nomesFuncionarios = f.getFuncPorId(func.Funcionario).nomeFunc;
                        }
                        pendentesModelo.Add(Aux);
                    }
                }
                return pendentesModelo;
            }
            catch (Exception e)
            {
                return new List<TarefaModelo>();
            }
        }

        public int getTarefasHojePendentes(string tipoTarefa)
        {
            List<Tarefas> pendentes = new List<Tarefas>();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    pendentes = (from Tarefas in BancoDeDados.Tarefas where Tarefas.inicioTarefa >= DateTime.Today where Tarefas.fimTarefa == null where Tarefas.tipoTarefa == tipoTarefa select Tarefas).ToList();
                }
                return pendentes.Count;
            }
            catch
            {
                return 0;
            }
        }

        public int getTarefasHojeFinalizadas(string tipoTarefa)
        {
            List<Tarefas> pendentes = new List<Tarefas>();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                { 
                    pendentes = (from Tarefas in BancoDeDados.Tarefas where Tarefas.inicioTarefa >= DateTime.Today where Tarefas.fimTarefa != null where Tarefas.tipoTarefa == tipoTarefa select Tarefas).ToList();
                }
                return pendentes.Count;
            }
            catch
            {
                return 0;
            }
        }

        public bool verificaDocumentoTarefa(int numDocumento, string tipoTarefa)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var cadastrado = BancoDeDados.Tarefas.FirstOrDefault(t => t.documentoTarefa == numDocumento && t.tipoTarefa == tipoTarefa);
                    if (cadastrado == null)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                return false;
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

                    // Have to rethink that, I know it will be hard, but you can do it
                    /*
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
                    }*/

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