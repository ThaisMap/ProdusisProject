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

        public List<TarefaModelo> getTarefasDivergencia(int tipo, int manifesto)
        {
            List<TarefaModelo> listaModelo = new List<TarefaModelo>();
            List<Tarefas> lista = new List<Tarefas>();

            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    if (tipo == 2)
                    {
                        var ctes = BancoDeDados.Cte_Manifesto.Where(m => m.Manifesto == manifesto).Select(c => c.Cte).ToList();
                        foreach (int cte in ctes)
                        {
                            lista.Add(BancoDeDados.Tarefas.Where(c => c.documentoTarefa == cte).FirstOrDefault());
                        }
                        listaModelo = tarefaModeloParse(lista);

                    }
                    else
                    {
                        lista.Add(BancoDeDados.Tarefas.Where(c => c.documentoTarefa == manifesto).FirstOrDefault());
                    }
                }
            }
            catch (Exception e)
            {
                var whatHapened = e;
            }

            return listaModelo;

        }

        public List<ItemRanking> rankingFuncionarios(List<TarefaModelo> tarefasPeriodo)
        {
            foreach (TarefaModelo tar in tarefasPeriodo)
            {
                tar.atualizaPontuação();
            }

            var lista = from t in tarefasPeriodo
                        group t by new
                        {
                            t.nomesFuncionarios
                        } into g
                        select new
                        {
                            Average = g.Average(p => p.pontosPorHora),
                            g.Key.nomesFuncionarios
                        };

            List<ItemRanking> Rank = new List<ItemRanking>();

            foreach(var item in lista)
            {
                Rank.Add(new ItemRanking(item.Average, item.nomesFuncionarios));
            }

            Rank=Rank.OrderBy(i => i.mediaPorHora).ToList();

            return Rank;
        }

        /// <summary>
        /// Retorna uma lista com as tarefas que atendam os filtros informados
        /// </summary>
        /// <param name="f">Parâmetros de pesquisa</param>
        public List<TarefaModelo> getTarefasFiltradas(Filtro f)
        {
            List<TarefaModelo> lista = new List<TarefaModelo>();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var query = BancoDeDados.Tarefas.AsQueryable();

                    if (f.dataFim != null)
                        query = query.Where(t => t.inicioTarefa <= f.dataFim);

                    if (f.dataInicio != null)
                        query = query.Where(t => t.inicioTarefa >= f.dataInicio);

                    if (f.numDocumento > 0)
                        query = query.Where(t => t.documentoTarefa == f.numDocumento);

                    if (f.TipoTarefa != "-1")
                        query = query.Where(t => t.tipoTarefa == f.TipoTarefa);

                    lista = tarefaModeloParse(query.ToList());
                }

                if (f.nomeFuncionario != "")
                    lista = lista.Where(l => l.nomesFuncionarios.Contains(f.nomeFuncionario)).ToList();

                if (f.volumeInicio > 0)
                    lista = lista.Where(l => l.volumes >= f.volumeInicio).ToList();

                if (f.volumeFim > 0)
                    lista = lista.Where(l => l.volumes <= f.volumeFim).ToList();

                if (f.skuInicio > 0)
                    lista = lista.Where(l => l.skus >= f.skuInicio).ToList();

                if (f.skuFim > 0)
                    lista = lista.Where(l => l.skus <= f.skuFim).ToList();

                if (f.pesoInicio > 0)
                    lista = lista.Where(l => l.peso >= f.pesoInicio).ToList();

                if (f.pesoFim > 0)
                    lista = lista.Where(l => l.peso <= f.pesoFim).ToList();

                foreach(var tar in lista)
                {
                    if (tar.tipoTarefa == "Conferência")
                    {
                        DocumentosBD d = new DocumentosBD();
                        tar.fornecedor = d.getFornecedorCte(tar.documentoTarefa);
                    }
                    else
                        tar.fornecedor = "";
                }
            }
            catch (Exception e)
            {
                var whatHapened = e;
            }

            return lista;
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
                        Aux.nomesFuncionarios = nomesFuncTarefa(Aux.idTarefa);
                        
                        pendentesModelo.Add(Aux);
                    }
                }
                return pendentesModelo;
            }
            catch
            {
                return new List<TarefaModelo>();
            }
        }

        public bool inserirDivergencia(List<TarefaModelo> listaDivergencia)
        {
            try
            {
                foreach (TarefaModelo tar in listaDivergencia)
                {
                    using (var BancoDeDados = new produsisBDEntities())
                    {
                        Tarefas tarefaAtual = BancoDeDados.Tarefas.Single(t => t.idTarefa == tar.idTarefa);
                        tarefaAtual.divergenciaTarefa = tar.divergenciaTarefa;
                        BancoDeDados.SaveChanges();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        public string nomesFuncTarefa(int idTarefa)
        {
            FuncionariosBD f = new FuncionariosBD();
            try
            {
                string nomes = null;
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var funcionarios = BancoDeDados.Func_Tarefa.Where(x => x.Tarefa == idTarefa).ToList();
                    foreach (var func in funcionarios)
                    {
                        if (nomes != null)
                            nomes += "/" + f.getFuncPorId(func.Funcionario).nomeFunc;
                        else
                            nomes = f.getFuncPorId(func.Funcionario).nomeFunc;
                    }
                }
                return nomes;
            }
            catch
            {
                return "";
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
            catch
            {
                return false;
            }
        }

        private List<TarefaModelo> tarefaModeloParse(List<Tarefas> tarefas)
        {
            List<TarefaModelo> modelos = new List<TarefaModelo>();
            TarefaModelo aux;
            DocumentosBD d = new DocumentosBD();
            Manifestos m;
            foreach (Tarefas tar in tarefas)
            {
                if (tar != null)
                {                   
                    aux = new TarefaModelo(tar);
                    if (tar.tipoTarefa == "2")
                    {
                        aux.valores(d.getSkuCte(tar.documentoTarefa), d.getVolumesCte(tar.documentoTarefa), (int)d.getPesoCte(tar.documentoTarefa));
                        aux.fornecedor = d.getFornecedorCte(tar.documentoTarefa);
                    }
                    else
                    {
                        m = d.getManifestoPorNumero(tar.documentoTarefa);
                        aux.valores(m.skusManifesto, m.VolumesManifesto, (int)m.pesoManifesto);
                    }
                    aux.nomesFuncionarios = nomesFuncTarefa(tar.idTarefa);
                    modelos.Add(aux);
                }
            }

            return modelos;
        }
    }
}