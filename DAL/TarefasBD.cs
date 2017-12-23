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
        public bool finalizarTarefa(int idTarefa, int quantPalet, int totalPalet)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Tarefas tarefaAtual = BancoDeDados.Tarefas.Single(f => f.idTarefa == idTarefa);
                    tarefaAtual.fimTarefa = DateTime.Now;
                    tarefaAtual.quantPaletizado = quantPalet;
                    tarefaAtual.totalPaletes = totalPalet;
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

        public TarefaModelo GetTarefaDivergencia(int tipo, int documento)
        {
            Tarefas tarefa = new Tarefas();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    tarefa = BancoDeDados.Tarefas.Where(t => t.documentoTarefa == documento).FirstOrDefault();
                    var tmodelo = tarefaModeloParse();
                }
            }
            catch
            {
                return tarefa;
            }
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

        public List<ItemRanking> rankingFuncionarios(List<ItemRanking> tarefasPeriodo, double horas)
        {
            DocumentosBD d = new DocumentosBD();
            
            var lista = from t in tarefasPeriodo
                        group t by new
                        {
                            t.nomesFuncionarios
                        } into g
                        select new
                        {
                            Sum = g.Sum(p => p.mediaPorHora),
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

        public List<ItemRanking> getRanking(Filtro f)
        {
            List<ItemRanking> listaFinal = new List<ItemRanking>();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var listaConf = BancoDeDados.RelatorioConferencias.Where(i => i.tipoTarefa == f.TipoTarefa)
                        .Where(i => i.inicioTarefa > f.dataInicio)
                        .Where(i => i.inicioTarefa < f.dataFim)
                        .ToList();
                    var listaNotConf = BancoDeDados.RelatorioNaoConferencia.Where(i => i.tipoTarefa == f.TipoTarefa)
                        .Where(i => i.inicioTarefa > f.dataInicio)
                        .Where(i => i.inicioTarefa < f.dataFim)
                        .ToList();

                    var listaItems = consolidarRelatorio(listaConf, listaNotConf, true);
                    var listaTarefas = listaItems.Select(i => i.idTarefa);
                    var resultado = BancoDeDados.Func_Tarefa.Where(t => listaTarefas.Contains(t.Tarefa)).ToList();
                    
                    string nome;
                    foreach (var item in listaItems)
                    {
                        item.atualizaPontuação();
                        inserirPontuacao(item.idTarefa, (float)item.pontos);
                    }

                    foreach (var item in resultado)
                    {
                       nome = BancoDeDados.Funcionarios.Where(func => func.idFunc == item.Funcionario).Select(func => func.nomeFunc).FirstOrDefault();
                        listaFinal.Add(new ItemRanking((double)item.Pontuacao, nome));
                    }
                }
                return listaFinal;
            }
            catch (Exception e)
            {
                var erro = e;
                return listaFinal;
            }
        }

        /// <summary>
        /// Retorna uma lista com as tarefas que atendam os filtros informados
        /// </summary>
        /// <param name="f">Parâmetros de pesquisa</param>
        public List<ItemRelatorio> getTarefasFiltradas(Filtro f, bool consolidarFuncionario)
        {
            List<ItemRelatorio> lista = new List<ItemRelatorio>();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    List<RelatorioNaoConferencia> relatorioNaoConferencia = new List<RelatorioNaoConferencia>();
                    List<RelatorioConferencias> relatorioConferencia = new List<RelatorioConferencias>();
     
                    #region filtrando Nao Conferencias
                    if (f.TipoTarefa !="2")
                    {
                        var query = BancoDeDados.RelatorioNaoConferencia.AsQueryable();

                        if (f.TipoTarefa != "-1")
                            query = query.Where(t => t.tipoTarefa == f.TipoTarefa);

                        if (f.dataFim != null)
                            query = query.Where(t => t.inicioTarefa <= f.dataFim);

                        if (f.dataInicio != null)
                            query = query.Where(t => t.inicioTarefa >= f.dataInicio);

                        if (f.numDocumento > 0)
                            query = query.Where(t => t.documentoTarefa == f.numDocumento);
                       
                        if (f.nomeFuncionario != "" && f.nomeFuncionario != null)

                            query = query.Where(l => l.nomeFunc == f.nomeFuncionario);

                        if (f.volumeInicio > 0)
                            query = query.Where(l => l.VolumesManifesto >= f.volumeInicio);

                        if (f.volumeFim > 0)
                            query = query.Where(l => l.VolumesManifesto <= f.volumeFim);

                        if (f.skuInicio > 0)
                            query = query.Where(l => l.skusManifesto >= f.skuInicio);

                        if (f.skuFim > 0)
                            query = query.Where(l => l.skusManifesto <= f.skuFim);

                        relatorioNaoConferencia = query.ToList();
                    }
                    #endregion
             
                    #region filtrando Conferencias
                    if (f.TipoTarefa == "2" || f.TipoTarefa == "-1")
                    {
                        var queryConf = BancoDeDados.RelatorioConferencias.AsQueryable();

                        if (f.TipoTarefa != "-1")
                            queryConf = queryConf.Where(t => t.tipoTarefa == f.TipoTarefa);

                        if (f.dataFim != null)
                            queryConf = queryConf.Where(t => t.inicioTarefa <= f.dataFim);

                        if (f.dataInicio != null)
                            queryConf = queryConf.Where(t => t.inicioTarefa >= f.dataInicio);

                        if (f.numDocumento > 0)
                            queryConf = queryConf.Where(t => t.documentoTarefa == f.numDocumento);
                  
                        if (f.nomeFuncionario != "" && f.nomeFuncionario != null)

                            queryConf = queryConf.Where(l => l.nomeFunc == f.nomeFuncionario);

                        if (f.volumeInicio > 0)
                            queryConf = queryConf.Where(l => l.volumesNF >= f.volumeInicio);

                        if (f.volumeFim > 0)
                            queryConf = queryConf.Where(l => l.volumesNF <= f.volumeFim);

                        if (f.skuInicio > 0)
                            queryConf = queryConf.Where(l => l.skuNF >= f.skuInicio);

                        if (f.skuFim > 0)
                            queryConf = queryConf.Where(l => l.skuNF <= f.skuFim);

                        relatorioConferencia = queryConf.ToList();
                    }

                    #endregion
                    
                    lista = consolidarRelatorio(relatorioConferencia, relatorioNaoConferencia, consolidarFuncionario);
                    lista.OrderBy(o => o.idTarefa);
                }
            }
            catch (Exception e)
            {
                var whatHapened = e;
            }
            return lista;
        }

        private List<ItemRelatorio> consolidarRelatorio(List<RelatorioConferencias> conferencias, List<RelatorioNaoConferencia> outros, bool consolidarFuncionario)
        {
            List<ItemRelatorio> lista = new List<ItemRelatorio>();
            DocumentosBD d = new DocumentosBD();
            Manifestos auxiliar = new Manifestos();

            if (consolidarFuncionario)
            {
                foreach (var tarefa in conferencias)
                {
                    var x = lista.Where(id => id.idTarefa == tarefa.idTarefa).Where(id => !(id.nomesFunc.Contains(tarefa.nomeFunc))).FirstOrDefault();
                    if (x == null)
                        lista.Add(new ItemRelatorio(tarefa));
                    else
                    {
                        int index = lista.IndexOf(x);
                        lista[index].nomesFunc = lista[index].nomesFunc + "/" + tarefa.nomeFunc;
                    }
                }
                
                foreach (var tarefa in outros)
                {
                    var x = lista.Where(id => id.idTarefa == tarefa.idTarefa).FirstOrDefault();
                    if (x == null)
                    {
                        auxiliar = d.getManifestoPorNumero(tarefa.documentoTarefa);
                        lista.Add(new ItemRelatorio(tarefa)
                        {
                            fornecedor = d.getFornecedorManifesto(tarefa.documentoTarefa),
                            ctesNoManifesto = auxiliar.quantCtesManifesto
                        });
                    }
                    else
                    {
                        int index = lista.IndexOf(x);
                        lista[index].nomesFunc = lista[index].nomesFunc + "/" + tarefa.nomeFunc;
                    }
                }
            }
            else
            {
                foreach (var tarefa in conferencias)
                {
                    lista.Add(new ItemRelatorio(tarefa));
                }
                foreach (var tarefa in outros)
                {
                    auxiliar = d.getManifestoPorNumero(tarefa.documentoTarefa);
                    lista.Add(new ItemRelatorio(tarefa)
                    {
                        fornecedor = d.getFornecedorManifesto(tarefa.documentoTarefa),
                        ctesNoManifesto = auxiliar.quantCtesManifesto
                    });
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

        public bool inserirPontuacao(int idTarefa, float pontos)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    List<Func_Tarefa> funcTarefas = BancoDeDados.Func_Tarefa.Where(i => i.Tarefa == idTarefa).ToList();
                    foreach (var item in funcTarefas)
                    {
                        item.Pontuacao = pontos;
                    }
                    BancoDeDados.SaveChanges();
                }
                return true;
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
                    modelos.Add(tarefaModeloParse(tar));
                }
            }
            return modelos;
        }

        private TarefaModelo tarefaModeloParse(Tarefas tarefas)
        {
            aux = new TarefaModelo(tarefas);
            if (tarefas.tipoTarefa == "2")
            {
                aux.valores(d.getSkuCte(tarefas.documentoTarefa), d.getVolumesCte(tarefas.documentoTarefa));
                aux.fornecedor = d.getFornecedorCte(tarefas.documentoTarefa);
            }
            else
            {
                m = d.getManifestoPorNumero(tarefas.documentoTarefa);
                aux.valores(m.skusManifesto, m.VolumesManifesto);
            }
            aux.nomesFuncionarios = nomesFuncTarefa(tarefas.idTarefa);

            return aux;
        }           
    }
}