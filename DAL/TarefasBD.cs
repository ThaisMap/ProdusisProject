using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public class TarefasBD
    {
        /// <summary>
        /// Insere um novo registro de tarefa no banco de dados e altera o status dos funcionários
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
                        ft = new Func_Tarefa()
                        {
                            Tarefa = novaTarefa.idTarefa,
                            Funcionario = i
                        };

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
                    if (tarefaAtual.divergenciaTarefa.Length > 120)
                        tarefaAtual.divergenciaTarefa = tarefaAtual.divergenciaTarefa.Remove(100);
                    BancoDeDados.SaveChanges();
                    FuncionariosBD func = new FuncionariosBD();
                    foreach (Func_Tarefa f in tarefaAtual.Func_Tarefa)
                    {
                        func.editarOcupacaoFuncionario(f.Funcionario, false);
                    }
                }
                return true;
            }
            catch { 
                return false;
            }
        }

        /// <summary>
        /// Retorna lista de tarefas referentes a um manifesto. Caso o tipo seja conferências.
        /// </summary>
        /// <param name="tipo">Tipo de tarefas desejadas</param>
        public List<TarefaModelo> getTarefasDivergencia(int tipo, int documento)
        {
            List<TarefaModelo> listaModelo = new List<TarefaModelo>();
            List<Tarefas> lista = new List<Tarefas>();

            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    if (tipo == 2)
                    {
                        //Se o tipo desejado for Conferência, retorna todas as tarefas com ctes relacionados com o manifesto dp cte informado
                        int manif = BancoDeDados.Cte_Manifesto.Where(c => c.Cte == documento).Select(m => m.Manifesto).FirstOrDefault();
                        var ctes = BancoDeDados.Cte_Manifesto.Where(m => m.Manifesto == manif).Select(c => c.Cte).ToList();
                        foreach (int cte in ctes)
                        {
                            lista.Add(BancoDeDados.Tarefas.Where(c => c.documentoTarefa == cte).FirstOrDefault());
                        }                        
                    } 

                    else
                    { //Se for outro tipo, retorna só aquele tipo e manifesto
                        lista.Add(BancoDeDados.Tarefas.Where(c => c.documentoTarefa == documento && c.tipoTarefa == tipo.ToString()).FirstOrDefault());
                    }
                    listaModelo = tarefaModeloParse(lista);
                }
            }
            catch (Exception e)
            {
                var whatHapened = e;
            }
            return listaModelo;
        }

        public List<Observacoes> observacoesFunc(int id, DateTime? inicio, DateTime fim)
        {
            List<Observacoes> lista = new List<Observacoes>();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var query = BancoDeDados.Observacoes.AsQueryable();

                    query = query.Where(t => t.FuncObs == id);
                    if (fim != null)
                        query = query.Where(t => t.DataObs <= fim);

                    if (inicio != null)
                        query = query.Where(t => t.DataObs >= inicio);

                    lista = query.ToList();
                }
            }
            catch (Exception e)
            {
                var whatHapened = e;
            }

            return lista;
        }

        public List<ItemRanking> rankingFuncionarios(List<TarefaModelo> tarefasPeriodo, double horas)
        {
            DocumentosBD d = new DocumentosBD();
            foreach (TarefaModelo tar in tarefasPeriodo)
            {
                tar.valores(d.getSkuCte(tar.documentoTarefa), d.getVolumesCte(tar.documentoTarefa));
               // tar.atualizaPontuação();
            }

            var lista = from t in tarefasPeriodo
                        group t by new
                        {
                            t.nomesFuncionarios
                        } into g
                        select new
                        {
                            Sum = g.Sum(p => p.pontos),
                            g.Key.nomesFuncionarios
                        };

            List<ItemRanking> Rank = new List<ItemRanking>();            

            foreach(var item in lista)
            {

                Rank.Add(new ItemRanking(item.Sum/horas, item.nomesFuncionarios));
            }

            Rank=Rank.OrderByDescending(i => i.mediaPorHora).ToList();

            return Rank;
        }

        public List<TarefaModelo> getTarefasFiltradasRanking(Filtro f)
        {
            List<TarefaModelo> lista = new List<TarefaModelo>();
            List<Tarefas> listaTarefas = new List<Tarefas>();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var query = BancoDeDados.Tarefas.AsQueryable();

                    if (f.dataInicio != null)
                        query = query.Where(t => t.inicioTarefa >= f.dataInicio);

                    query = query.Where(t => t.inicioTarefa <= f.dataFim);
                    query = query.Where(t => t.tipoTarefa == f.TipoTarefa);

                    listaTarefas = query.ToList();
                }

                foreach (var tar in listaTarefas)
                {
                    lista.Add(new TarefaModelo(tar)
                    {
                        nomesFuncionarios = nomesFuncTarefa(tar.idTarefa),      
                    });
                }
            }
            catch (Exception e)
            {
                var whatHapened = e;
            }

            return lista;
        }

        /// <summary>
        /// Retorna uma lista com as tarefas que atendam os filtros informados
        /// </summary>
        /// <param name="f">Parâmetros de pesquisa</param>
        public List<ItemRelatorio> getTarefasFiltradas(Filtro f)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
#region filtrando Nao Conferencias
                    var query = BancoDeDados.RelatorioNaoConferencia.AsQueryable();
                    if (f.dataFim != null)
                        query = query.Where(t => t.inicioTarefa <= f.dataFim);

                    if (f.dataInicio != null)
                        query = query.Where(t => t.inicioTarefa >= f.dataInicio);

                    if (f.numDocumento > 0)
                        query = query.Where(t => t.documentoTarefa == f.numDocumento);

                    if (f.TipoTarefa != "-1")
                        query = query.Where(t => t.tipoTarefa == f.TipoTarefa);

                    if (f.nomeFuncionario != "" && f.nomeFuncionario != null)

                        query = query.Where(l => l.nomeFunc== f.nomeFuncionario);

                    if (f.volumeInicio > 0)
                        query = query.Where(l => l.VolumesManifesto >= f.volumeInicio);

                    if (f.volumeFim > 0)
                        query = query.Where(l => l.VolumesManifesto <= f.volumeFim);

                    if (f.skuInicio > 0)
                        query = query.Where(l => l.skusManifesto >= f.skuInicio);

                    if (f.skuFim > 0)
                        query = query.Where(l => l.skusManifesto <= f.skuFim);

                    var relatorioNaoConferencia = query.ToList();
                    #endregion

                    #region filtrando Conferencias
                    var queryConf = BancoDeDados.RelatorioConferencias.AsQueryable();
                    if (f.dataFim != null)
                        queryConf = queryConf.Where(t => t.inicioTarefa <= f.dataFim);

                    if (f.dataInicio != null)
                        queryConf = queryConf.Where(t => t.inicioTarefa >= f.dataInicio);

                    if (f.numDocumento > 0)
                        queryConf = queryConf.Where(t => t.documentoTarefa == f.numDocumento);

                    if (f.TipoTarefa != "-1")
                        queryConf = queryConf.Where(t => t.tipoTarefa == f.TipoTarefa);

                    if (f.nomeFuncionario != "" && f.nomeFuncionario != null)

                        queryConf = queryConf.Where(l => l.nomeFunc == f.nomeFuncionario);

                    if (f.volumeInicio > 0)
                        queryConf = queryConf.Where(l => l.VolumesManifesto >= f.volumeInicio);

                    if (f.volumeFim > 0)
                        queryConf = queryConf.Where(l => l.VolumesManifesto <= f.volumeFim);

                    if (f.skuInicio > 0)
                        queryConf = queryConf.Where(l => l.skusManifesto >= f.skuInicio);

                    if (f.skuFim > 0)
                        queryConf = queryConf.Where(l => l.skusManifesto <= f.skuFim);

                    var relatorioConferencia = queryConf.ToList();
                    #endregion

                    List<ItemRelatorio> lista = consolidarRelatorio(relatorioConferencia, relatorioNaoConferencia);

                }
            }
            catch (Exception e)
            {
                var whatHapened = e;
            }
            return lista;
        }

        private List<ItemRelatorio> consolidarRelatorio(List<RelatorioConferencias> conferencias, List<RelatorioNaoConferencia> outros)
        {
            List<ItemRelatorio> lista = new List<ItemRelatorio>();

            foreach(var tarefa in conferencias)
            {
                var x = lista.Where(id => id.idTarefa == tarefa.idTarefa).FirstOrDefault();
                if(x == null)
                lista.Add(new ItemRelatorio(tarefa));
                else
                {
                    int index = lista.IndexOf(x);
                    lista[index].nomesFunc = lista[index].nomesFunc + "/" + tarefa.nomeFunc;
                }
            }
            DocumentosBD d = new DocumentosBD();
            foreach (var tarefa in outros)
            {
                var x = lista.Where(id => id.idTarefa == tarefa.idTarefa).FirstOrDefault();
                if (x == null)
                    lista.Add(new ItemRelatorio(tarefa){ fornecedor =  d.getFornecedorManifesto(tarefa.documentoTarefa)});
                else
                {
                    int index = lista.IndexOf(x);
                    lista[index].nomesFunc = lista[index].nomesFunc + "/" + tarefa.nomeFunc;
                }
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

            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    pendentes = (from Tarefas in BancoDeDados.Tarefas where Tarefas.fimTarefa == null where Tarefas.tipoTarefa == tipoTarefa select Tarefas).ToList();
                    pendentesModelo = tarefaModeloParse(pendentes);
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
                        aux.valores(d.getSkuCte(tar.documentoTarefa), d.getVolumesCte(tar.documentoTarefa));
                        aux.fornecedor = d.getFornecedorCte(tar.documentoTarefa);
                    }
                    else
                    {
                        m = d.getManifestoPorNumero(tar.documentoTarefa);
                        aux.valores(m.skusManifesto, m.VolumesManifesto);
                    }
                    aux.nomesFuncionarios = nomesFuncTarefa(tar.idTarefa);
                    modelos.Add(aux);
                }
            }
            return modelos;
        }        
    }
}