using DAL.Properties;
using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class AcessoBD
    {
        #region Cadastros
        
        #region Documentos
        public void CadastrarCte(Cte novoCte)
        {
            try
            {
                if (novoCte.notasCte.Contains("\\"))
                {
                    var separar = novoCte.notasCte.Split('\\');
                    separar = separar.OrderBy(x => x).ToArray();
                    novoCte.notasCte = "";
                    foreach (var item in separar)
                    {
                        novoCte.notasCte += item + "\\";
                    }
                    novoCte.notasCte = novoCte.notasCte.TrimEnd('\\');
                }
                using (var BancoDeDados = new produsisBDEntities())
                {
                    if (!CteExiste(novoCte.numeroCte, novoCte.notasCte))
                    {
                        BancoDeDados.Cte.Add(novoCte);
                        BancoDeDados.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Insere  um registro de cte em manifesto no banco de dados
        /// </summary>
        /// <param name="novo">Dados do novo registro</param>
        public void CadastrarCteManifesto(Cte_Manifesto novo)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.Cte_Manifesto.Add(novo);
                    BancoDeDados.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Insere um registro de manifesto no banco de dados
        /// </summary>
        /// <param name="novoManifesto">Dados do novo registro</param>
        /// <returns>True se o comando foi executado sem erros, False se houve algum erro</returns>
        public void CadastrarManifesto(Manifestos novoManifesto)
        {
            using (var BancoDeDados = new produsisBDEntities())
            {
                try
                {
                    if (ManifestoExiste(novoManifesto.numeroManifesto))
                    {
                        var manifestoAlterado = BancoDeDados.Manifestos.Where(x => x.numeroManifesto == novoManifesto.numeroManifesto).First();
                        manifestoAlterado.pesoManifesto = novoManifesto.pesoManifesto;
                        manifestoAlterado.quantCtesManifesto = novoManifesto.quantCtesManifesto;
                        manifestoAlterado.VolumesManifesto = novoManifesto.VolumesManifesto;

                        BancoDeDados.SaveChanges();
                    }
                    else
                    {
                        BancoDeDados.Manifestos.Add(novoManifesto);

                        BancoDeDados.SaveChanges();
                    }
                }
                catch
                {

                }
            }
        }

        public void CadastrarNF(NotasFiscais novaNf)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.NotasFiscais.Add(novaNf);
                    BancoDeDados.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Documentos

        #region Pessoas

        public bool CadastrarFuncionario(Funcionarios novoFunc)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.Funcionarios.Add(novoFunc);
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CadastrarObservacao(Observacoes novaObs)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.Observacoes.Add(novaObs);
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool SalvaEquipe(List<string> equipe, int? numEquipe)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var funcEquipe = BancoDeDados.Funcionarios.Where(x => equipe.Contains(x.nomeFunc));
                    foreach (var item in funcEquipe)
                    {
                        item.equipeFunc = numEquipe;
                    }
                    BancoDeDados.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }


        #endregion Pessoas

        #region Veiculos

        public bool CadastrarVeiculo(Veiculos novoVeiculo)
        {
            var foiCadastrado = true;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    if (BancoDeDados.Veiculos.Where(x => x.PlacaVeiculo == novoVeiculo.PlacaVeiculo).Any())
                    {
                        foiCadastrado = false;
                        var VeiculoAlterado = BancoDeDados.Veiculos.Where(x => x.PlacaVeiculo == novoVeiculo.PlacaVeiculo).First();
                        VeiculoAlterado.AtivoVeiculo = novoVeiculo.AtivoVeiculo;
                        VeiculoAlterado.CapacidadePaletes = novoVeiculo.CapacidadePaletes;
                        VeiculoAlterado.MotoristaVeiculo = novoVeiculo.MotoristaVeiculo;
                        VeiculoAlterado.Placa2Veiculo = novoVeiculo.Placa2Veiculo;
                        VeiculoAlterado.TipoVeiculo = novoVeiculo.TipoVeiculo;
                    }
                    else
                    {
                        BancoDeDados.Veiculos.Add(novoVeiculo);
                    }
                    BancoDeDados.SaveChanges();
                }
                return foiCadastrado;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void CadastrarCarretas(Carretas novaCarreta)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    if (BancoDeDados.Carretas.Where(x => x.PlacaCarreta == novaCarreta.PlacaCarreta).Any())
                    {
                        var VeiculoAlterado = BancoDeDados.Carretas.Where(x => x.PlacaCarreta == novaCarreta.PlacaCarreta).First();
                        VeiculoAlterado.Ativo = novaCarreta.Ativo;
                    }
                    else
                    {
                        BancoDeDados.Carretas.Add(novaCarreta);
                    }
                    BancoDeDados.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public bool CadastrarAcessoPortaria(AcessosPortaria novo)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.AcessosPortaria.Add(novo);
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion Veiculos

        #region Tarefas

        public bool CadastrarTarefa(Tarefas novaTarefa, int[] funcionarios)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    BancoDeDados.Tarefas.Add(novaTarefa);
                    BancoDeDados.SaveChanges();

                    foreach (int i in funcionarios)
                    {
                        CadastrarFunc_Tarefa(i, novaTarefa.idTarefa);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void CadastrarFunc_Tarefa(int idFuncionario, int idTarefa)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Func_Tarefa ft;

                    ft = new Func_Tarefa()
                    {
                        Tarefa = idTarefa,
                        Funcionario = idFuncionario
                    };

                    BancoDeDados.Func_Tarefa.Add(ft);
                    BancoDeDados.SaveChanges();
                    AlterarOcupacaoFuncionario(idFuncionario, true);
                }
            }
            catch
            {
            }
        }

        public void CadastrarNovaDivergencia(Divergencias novaDivergencia)
        {
            try
            {
                if (novaDivergencia.idDivergencia == 0)
                {
                    using (var BancoDeDados = new produsisBDEntities())
                    {
                        BancoDeDados.Divergencias.Add(novaDivergencia);
                        BancoDeDados.SaveChanges();
                    }
                }
                else
                {
                    using (var BancoDeDados = new produsisBDEntities())
                    {
                        var div = BancoDeDados.Divergencias.Where(x => x.idDivergencia == novaDivergencia.idDivergencia).FirstOrDefault();
                        div.QtdeDivergencia = novaDivergencia.QtdeDivergencia;
                        div.TextoDivergencia = novaDivergencia.TextoDivergencia;
                        div.TipoDivergencia = novaDivergencia.TipoDivergencia;
                        BancoDeDados.SaveChanges();
                    }
                }
            }
            catch
            {
            }
        }

      

        #endregion Tarefas

        #endregion Cadastros

        //-----------------------------------------------------------------------

        #region Deletes

        public void DeletarObservacao(int idObservacao)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Observacoes apagar = BancoDeDados.Observacoes.Where(i => i.idObs == idObservacao).FirstOrDefault();
                    BancoDeDados.Observacoes.Remove(apagar);
                    BancoDeDados.SaveChanges();
                }
            }
            catch
            {
            }
        }

        public void ApagarNovaDivergencia(int id)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var div = BancoDeDados.Divergencias.Where(x => x.idDivergencia == id).FirstOrDefault();
                    BancoDeDados.Divergencias.Remove(div);
                    BancoDeDados.SaveChanges();
                }
            }
            catch (Exception e)
            {
            }
        }

        #endregion Deletes

        //-----------------------------------------------------------------------

        #region Gets

        #region Documentos

        public Cte GetCtePorID(int idCte)
        {
            Cte novo = new Cte();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    novo = (from Cte in BancoDeDados.Cte where Cte.idCte == idCte select Cte).FirstOrDefault();
                }
                return novo;
            }
            catch
            {
                return novo;
            }
        }

        private string GetDadosNovoCte(int numDoc)
        {
            string dados;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Cte CteAtual = (from Cte in BancoDeDados.Cte where Cte.numeroCte == numDoc select Cte).OrderByDescending(x => x.idCte).FirstOrDefault();
                    dados = "Cte n º " + CteAtual.numeroCte + " - " + GetVolumesCte(CteAtual.idCte) + " volumes - " + GetSkuCte(CteAtual.idCte) + " SKU's";
                }
            }
            catch
            {
                dados = "Dados não encontrados";
            }
            return dados;
        }

        public List<Cte> GetNovoCtePorNum(int numCte)
        {
            List<Cte> lista = new List<Cte>();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    lista = (from Cte in BancoDeDados.Cte where Cte.numeroCte == numCte select Cte).ToList();
                }
            }
            catch
            {
            }
            return lista;
        }

        public int CtesImportadosNoManifesto(int numManifesto)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var novosCtes = from manifestados in BancoDeDados.Cte_Manifesto
                                    from importados in BancoDeDados.NotasFiscais
                                    where manifestados.Manifesto == numManifesto
                                    where manifestados.CteNovo == importados.CteNovoNF
                                    select manifestados;
                    novosCtes = novosCtes.Distinct();
                    return novosCtes.Count();
                }
            }
            catch
            {
                return -1;
            }
        }

        public int CtesConferidosNoManifesto(int numManifesto)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var novosCtes = from manifestados in BancoDeDados.Cte_Manifesto
                                    from conferidos in BancoDeDados.Tarefas
                                    where manifestados.Manifesto == numManifesto
                                    where manifestados.CteNovo == conferidos.documentoTarefa
                                    where conferidos.fimTarefa != null
                                    select manifestados;
                    return novosCtes.Count();
                }
            }
            catch
            {
                return -1;
            }
        }

        public string GetListaManifestosCte(int numCte)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var manifestos = (from Cte_Manifesto in BancoDeDados.Cte_Manifesto where Cte_Manifesto.CteNovo == numCte select Cte_Manifesto).ToList();
                    if (manifestos != null)
                    {
                        string notas = "";
                        foreach (var item in manifestos)
                        {
                            notas = item.Manifesto.ToString() + "/";
                        }
                        notas = notas.Trim('/');
                        return notas;
                    }
                    return "Não foi encontrado.";
                }
            }
            catch
            {
                return "Não foi encontrado.";
            }
        }

        public string GetDadosCte(int numDoc)
        {
            string dados;
            try
            {
                dados = "Cte n º " + numDoc + " - " + GetVolumesCte(numDoc) + " volumes - " + GetSkuCte(numDoc) + " SKU's";
            }
            catch
            {
                dados = "Dados não encontrados";
            }
            return dados;
        }

        public string GetDadosManifesto(int numDoc)
        {
            string dados;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Manifestos m = BancoDeDados.Manifestos.SingleOrDefault(man => man.numeroManifesto == numDoc);
                    dados = "Manifesto nº " + m.numeroManifesto + " - " + m.quantCtesManifesto + " cte's - " + m.VolumesManifesto + " volumes - " + m.pesoManifesto + " Kg";
                }
            }
            catch
            {
                dados = "Dados não encontrados";
            }
            return dados;
        }

        public string GetFornecedorCte(int idCte)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var nota = (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.CteNovoNF == idCte select NotasFiscais).FirstOrDefault();
                    return nota.fornecedorNF;
                }
            }
            catch (Exception erro)
            {
                return "Fornecedor não encontrado";
            }
        }

        public string GetFornecedorManifesto(int numManifesto)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var result = (from a in BancoDeDados.NotasFiscais
                                  join b in BancoDeDados.Cte_Manifesto on a.CteNovoNF equals b.CteNovo
                                  where b.Manifesto == numManifesto
                                  select a.fornecedorNF).ToList();
                    if (result.Count == 0)
                    {
                        return "Fornecedor não encontrado";
                    }
                    if (result.Count == 1)
                    {
                        return result[0];
                    }
                    if (result.Any(o => o != result[0]))
                        return "VARIOS FORNECEDORES";
                    else
                        return result[0];
                }
            }
            catch (Exception e)
            {
                return "Fornecedor não encontrado";
            }
        }

        public Manifestos GetManifestoPorNumero(int numManifesto)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Manifestos in BancoDeDados.Manifestos where Manifestos.numeroManifesto == numManifesto select Manifestos).FirstOrDefault();
                }
            }
            catch
            {
                return new Manifestos();
            }
        }

        public List<NotasFiscais> GetNFPorNumero(string numNF)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (
                        from NotasFiscais in BancoDeDados.NotasFiscais
                        where NotasFiscais.numeroNF.StartsWith(numNF)
                        orderby NotasFiscais.numeroNF
                        select NotasFiscais).ToList();
                }
            }
            catch
            {
                return new List<NotasFiscais>();
            }
        }

        public int GetSkuCte(int idCte)
        {
            int sku = 0;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    List<int> ListaNFs = (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.CteNovoNF == idCte select NotasFiscais.skuNF).ToList();
                    sku = ListaNFs.Sum();
                }
            }
            catch
            {
                return -1;
            }
            return sku;
        }

        public int GetVolumesCte(int numeroCte)
        {
            int volumes = 0;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    List<int> ListaNFs = (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.CteNovoNF == numeroCte select NotasFiscais.volumesNF).ToList();
                    volumes = ListaNFs.Sum();
                    return volumes;
                }
            }
            catch
            {
                return -1;
            }
        }

        #endregion Documentos

        #region Pessoas

        public List<Observacoes> GetObservacoes(DateTime? inicio, DateTime? fim)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var lista = (from Observacoes in BancoDeDados.Observacoes where Observacoes.DataObs <= fim select Observacoes);
                    if (inicio != null)
                        lista = lista.Where(d => d.DataObs >= inicio);

                    foreach (var item in lista)
                    {
                        item.NomeFunc = item.Funcionarios.nomeFunc;
                    }

                    return lista.ToList();
                }
            }
            catch (Exception e)
            {
                return new List<Observacoes>();
            }
        }

        public List<Funcionarios> GetFuncionariosLivres(string tipo)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Funcionarios in BancoDeDados.Funcionarios where Funcionarios.ativoFunc == true where Funcionarios.ocupadoFunc == false where Funcionarios.tipoFunc.Contains(tipo) orderby Funcionarios.nomeFunc select Funcionarios).ToList();
                }
            }
            catch
            {
                return new List<Funcionarios>();
            }
        }

        /// <summary>
        /// Conferente não tem equipe, então pode ser só a lista dos nomes
        /// </summary>
        public List<string> GetConferentesLivres(string tipo)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Funcionarios in BancoDeDados.Funcionarios where Funcionarios.ativoFunc == true where Funcionarios.ocupadoFunc == false where Funcionarios.tipoFunc.Contains(tipo) orderby Funcionarios.nomeFunc select Funcionarios.nomeFunc).ToList();
                }
            }
            catch
            {
                return new List<string>();
            }
        }

        public Funcionarios GetFuncPorMatricula(string Matricula)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return BancoDeDados.Funcionarios.Where(f => f.matriculaFunc == Matricula).FirstOrDefault();
                }
            }
            catch
            {
                return new Funcionarios();
            }
        }

        public Funcionarios GetFuncPorNome(string nomeUsuario)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Funcionarios in BancoDeDados.Funcionarios where Funcionarios.nomeFunc == nomeUsuario select Funcionarios).FirstOrDefault();
                }
            }
            catch
            {
                return new Funcionarios();
            }
        }

        public List<string> GetListaNomesFunc()
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Funcionarios in BancoDeDados.Funcionarios orderby Funcionarios.nomeFunc select Funcionarios.nomeFunc).ToList();
                }
            }
            catch (Exception e)
            {
                return new List<string> { e.Message };
            }
        }

        public List<Funcionarios> GetFuncionariosAtivos()
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Funcionarios in BancoDeDados.Funcionarios
                            where Funcionarios.ativoFunc == true
                            orderby Funcionarios.nomeFunc
                            select Funcionarios).ToList();
                }
            }
            catch
            {
                return new List<Funcionarios>();
            }
        }

        #endregion Pessoas

        #region Veiculos

        public List<Carretas> GetCarretas()
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var lista = BancoDeDados.Carretas.OrderBy(x => x.PlacaCarreta).ToList();
                    return lista;
                }
            }
            catch (Exception erro)
            {
                return new List<Carretas>();
            }
        }

        public List<Veiculos> GetVeiculos()
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var lista = BancoDeDados.Veiculos.OrderBy(x => x.PlacaVeiculo).ToList();
                    return lista;
                }
            }
            catch (Exception erro)
            {
                return new List<Veiculos>();
            }
        }

        public List<Carretas> GetPlaca2()
        {
            List<Carretas> lista;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    lista = BancoDeDados.Carretas.Where(x => x.Ativo == true).ToList();
                }
                return lista;
            }
            catch (Exception)
            {
                return new List<Carretas>();
            }
        }

        public List<AcessosPortaria> FiltrarAcessos(Filtro f)
        {
            List<AcessosPortaria> acessosFiltrados;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var query = BancoDeDados.AcessosPortaria.AsQueryable();
                    if (f.acessoPendente == true)
                        query = query.Where(a => a.SaidaAcesso == null);
                    if (f.dataInicio != null)
                        query = query.Where(a => a.EntradaAcesso >= f.dataInicio);
                    if (f.dataFim != null)
                        query = query.Where(a => a.EntradaAcesso <= f.dataFim);
                    if (f.nomeFuncionario != "")
                        query = query.Where(a => a.NomeMotoristaAcesso.Contains(f.nomeFuncionario));
                    if (f.placa != "")
                        query = query.Where(a => a.PlacaAcesso == f.placa);
                    acessosFiltrados = query.ToList();
                }

                return acessosFiltrados;
            }
            catch (Exception)
            {
                return new List<AcessosPortaria>();
            }
        }

        public List<AcessosPortaria> GetAcessosPendentes()
        {
            List<AcessosPortaria> acessosPendentes;
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    acessosPendentes = (from AcessosPortaria in BancoDeDados.AcessosPortaria where AcessosPortaria.SaidaAcesso == null select AcessosPortaria).ToList();
                }
                return acessosPendentes;
            }
            catch (Exception)
            {
                return new List<AcessosPortaria>();
            }
        }

        public AcessosPortaria GetAcessoPorID(int idAcesso)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from AcessosPortaria in BancoDeDados.AcessosPortaria where AcessosPortaria.idAcesso == idAcesso select AcessosPortaria).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return new AcessosPortaria();
            }
        }

        #endregion Veiculos

        #region Tarefas

        public string GetNomesFuncTarefa(int idTarefa)
        {
            try
            {
                string nomes = "";
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var funcionarios = BancoDeDados.Func_Tarefa.Where(x => x.Tarefa == idTarefa);
                    foreach (var func in funcionarios)
                    {
                        nomes += func.Funcionarios.nomeFunc + "/";
                    }
                    nomes = nomes.TrimEnd('/');
                }
                return nomes;
            }
            catch
            {
                return "";
            }
        }
        
        public List<Divergencias> GetNovaDivergencia(int documento, int tipo)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Tarefas tarefa;
                    if (tipo != 2)
                    {
                        tarefa = BancoDeDados.Tarefas.Where(t => t.documentoTarefa == documento && t.tipoTarefa == tipo.ToString()).FirstOrDefault();
                    }
                    else
                    {
                        var idCte = BancoDeDados.Cte.Where(c => c.numeroCte == documento).Select(c => c.idCte).OrderByDescending(x => x).FirstOrDefault();
                        tarefa = BancoDeDados.Tarefas.Where(t => t.documentoTarefa == idCte).FirstOrDefault();
                    }
                    return BancoDeDados.Divergencias.Where(d => d.TarefaDivergencia == tarefa.idTarefa).ToList();
                }
            }
            catch (Exception e)
            {
                return new List<Divergencias>();
            }
        }

        public TarefaModelo GetTarefaDivergencia(int tipo, int documento)
        {
            Tarefas tarefa = new Tarefas();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    if (tipo != 2)
                    {
                        tarefa = BancoDeDados.Tarefas.Where(t => t.documentoTarefa == documento).FirstOrDefault();
                    }
                    else
                    {
                        var idCte = BancoDeDados.Cte.Where(c => c.numeroCte == documento).Select(c => c.idCte).OrderByDescending(X => X).FirstOrDefault();
                        tarefa = BancoDeDados.Tarefas.Where(t => t.documentoTarefa == idCte).FirstOrDefault();
                    }
                }
                var tmodelo = TarefaModeloParse(tarefa);
                return tmodelo;
            }
            catch
            {
                return new TarefaModelo(tarefa);
            }
        }

        public int? GetPaletesSeparacao(int Premanifesto)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return BancoDeDados.Tarefas.Where(x => x.documentoTarefa == Premanifesto && x.tipoTarefa == "3").Select(x => x.totalPaletes).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }

        public List<TarefaModelo> GetTarefasPendentes(string tipoTarefa)
        {
            List<Tarefas> pendentes = new List<Tarefas>();
            List<TarefaModelo> pendentesModelo = new List<TarefaModelo>();

            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    pendentes = (from Tarefas in BancoDeDados.Tarefas where Tarefas.fimTarefa == null where Tarefas.tipoTarefa == tipoTarefa select Tarefas).ToList();
                }

                pendentesModelo = TarefaModeloParse(pendentes);                
                return pendentesModelo;
            }
            catch (Exception)
            {
                return new List<TarefaModelo>();
            }
        }

        public List<ItemRanking> GetRanking(Filtro f)
        {
            List<ItemRanking> listaFinal = new List<ItemRanking>();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var listaConf = BancoDeDados.RelatorioNovoConferencias.Where(i => i.tipoTarefa == f.TipoTarefa)
                        .Where(i => i.inicioTarefa >= f.dataInicio)
                        .Where(i => i.inicioTarefa <= f.dataFim)
                        .ToList();
                    var listaNotConf = BancoDeDados.RelatorioNaoConferencia.Where(i => i.tipoTarefa == f.TipoTarefa)
                        .Where(i => i.inicioTarefa >= f.dataInicio)
                        .Where(i => i.inicioTarefa <= f.dataFim)
                        .Where(i => i.fimTarefa != null)
                        .ToList();

                    var listaItems = ConsolidarRelatorio(listaConf, listaNotConf);
                    var listaTarefas = listaItems.Select(i => i.idTarefa);
                    var resultado = BancoDeDados.Func_Tarefa.Where(t => listaTarefas.Contains(t.Tarefa)).ToList();

                    string nome;
                    int qtde = 0;
                    foreach (var item in listaItems)
                    {
                        if (TemDivergencia(item.idTarefa))
                            InserirPontuacao(item.idTarefa, 0);
                        else
                        {
                            item.atualizaPontuação();
                            InserirPontuacao(item.idTarefa, (float)item.pontos);
                        }
                    }

                    foreach (var item in resultado)
                    {
                        nome = BancoDeDados.Funcionarios.Where(func => func.idFunc == item.Funcionario).Select(func => func.nomeFunc).FirstOrDefault();
                        listaFinal.Add(new ItemRanking((double)item.Pontuacao, nome, qtde));
                    }
                }
                return listaFinal;
            }
            catch (Exception e)
            {
                return listaFinal;
            }
        }
        
        public List<ItemRelatorio> GetTarefasFiltradas(Filtro f)
        {
            List<ItemRelatorio> lista = new List<ItemRelatorio>();
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    List<RelatorioNaoConferencia> relatorioNaoConferencia = new List<RelatorioNaoConferencia>();
                    List<RelatorioNovoConferencias> relatorioConferencia = new List<RelatorioNovoConferencias>();

                    #region filtrando Nao Conferencias

                    if (f.TipoTarefa != "2")
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

                        relatorioNaoConferencia = query.ToList();
                    }

                    #endregion filtrando Nao Conferencias

                    #region filtrando Conferencias

                    if (f.TipoTarefa == "2" || f.TipoTarefa == "-1")
                    {
                        var queryConf = BancoDeDados.RelatorioNovoConferencias.AsQueryable();

                        if (f.TipoTarefa != "-1")
                            queryConf = queryConf.Where(t => t.tipoTarefa == f.TipoTarefa);

                        if (f.dataFim != null)
                            queryConf = queryConf.Where(t => t.inicioTarefa <= f.dataFim);

                        if (f.dataInicio != null)
                            queryConf = queryConf.Where(t => t.inicioTarefa >= f.dataInicio);

                        if (f.numDocumento > 0)
                            queryConf = queryConf.Where(t => t.numeroCte == f.numDocumento);

                        if (f.nomeFuncionario != "" && f.nomeFuncionario != null)

                            queryConf = queryConf.Where(l => l.nomeFunc == f.nomeFuncionario);

                        if (f.volumeInicio > 0)
                            queryConf = queryConf.Where(l => l.Volumes >= f.volumeInicio);

                        if (f.volumeFim > 0)
                            queryConf = queryConf.Where(l => l.Volumes <= f.volumeFim);

                        if (f.skuInicio > 0)
                            queryConf = queryConf.Where(l => l.SKU >= f.skuInicio);

                        if (f.skuFim > 0)
                            queryConf = queryConf.Where(l => l.SKU <= f.skuFim);

                        relatorioConferencia = queryConf.ToList();
                    }

                    #endregion filtrando Conferencias

                    lista = ConsolidarRelatorio(relatorioConferencia, relatorioNaoConferencia);
                    lista.OrderBy(o => o.idTarefa);
                }
            }
            catch (Exception e)
            {
            }
            return lista;
        }
        
        #endregion Tarefas

        #endregion Gets

        //-----------------------------------------------------------------------

        #region Verificacoes

        #region Documentos

        /// <summary>
        /// Verifica se está cadastrado um cte com o numero e notas fiscais fornecidos
        /// </summary>
        /// <returns>true para erros, false se não houver correspondencia, true se houver correspondencia</returns>
        public bool CteExiste(int nCte, string nfs)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Cte in BancoDeDados.Cte where Cte.numeroCte == nCte where Cte.notasCte == nfs select Cte.idCte).Any();
                }
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// verifica se ja foi criada a relação cte_Manifesto
        /// </summary>
        /// <param name="novo"></param>
        /// <returns>true para erros, false se não houver correspondencia, true se houver correspondencia</returns>
        public bool CteManifestoExiste(Cte_Manifesto novo)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Cte_Manifesto in BancoDeDados.Cte_Manifesto
                            where Cte_Manifesto.Manifesto == novo.Manifesto
                            where Cte_Manifesto.CteNovo == novo.CteNovo
                            select Cte_Manifesto).Any();
                }
            }
            catch (Exception e)
            {
                var erro = e;
                return true;
            }
        }

        /// <summary>
        /// Verifica se o manifesto indicado esta cadastrado no banco de dados
        /// </summary>
        /// <returns>True se este manifesto for encontrado, False se nao for, True se houver erro</returns>
        public bool ManifestoExiste(int numManifesto)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Manifestos in BancoDeDados.Manifestos where Manifestos.numeroManifesto == numManifesto select Manifestos.numeroManifesto).Any();
                }
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// Verifica se a nota fiscal indicada esta cadastrada no banco de dados
        /// </summary>
        /// <returns>True se a NF for encontrada, False se nao for, True se houver erro</returns>
        public bool NfExiste(string numNf)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                   
                      return (from NotasFiscais in BancoDeDados.NotasFiscais where NotasFiscais.numeroNF == numNf select NotasFiscais.idNF).Any();                  
                }
            }
            catch
            {
                return true;
            }
        }

        #endregion Documentos

        #region Pessoas

        public bool SenhaCorreta(string matricula, string senha)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var senhaBD = (from Funcionarios in BancoDeDados.Funcionarios
                                   where Funcionarios.matriculaFunc == matricula
                                   select Funcionarios.senhaFunc).FirstOrDefault();

                    if (senhaBD == senha)
                    {
                        return true;
                    }
                    else return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool UsuarioExiste(string matricula)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Funcionarios in BancoDeDados.Funcionarios
                            where Funcionarios.matriculaFunc == matricula
                            select Funcionarios.tipoFunc).Any();              
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }


        #endregion Pessoas

        #region Tarefas

        public bool VerificaDocumentoTarefa(int numDocumento, string tipoTarefa)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                   return BancoDeDados.Tarefas.Any(t => t.documentoTarefa == numDocumento && t.tipoTarefa == tipoTarefa);                  
                }
            }
            catch
            {
                return false;
            }
        }

        private bool TemDivergencia(int idTarefa)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return BancoDeDados.Divergencias.Any(t => t.TarefaDivergencia == idTarefa);                   
                }
            }
            catch
            {
                return false;
            }
        }


        #endregion Tarefas

        #endregion Verificacoes

        //-----------------------------------------------------------------------

        #region Alteracoes

        #region Documentos

        public void AlterarPreManifesto(Manifestos novoManifesto)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Manifestos atual = BancoDeDados.Manifestos.Where(x => x.numeroManifesto == novoManifesto.numeroManifesto).FirstOrDefault();
                    atual.quantCtesManifesto = novoManifesto.quantCtesManifesto;
                    atual.VolumesManifesto = novoManifesto.VolumesManifesto;
                    BancoDeDados.SaveChanges();
                }
            }
            catch
            {
            }
        }

        public void AlterarNF(NotasFiscais novaNf)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var nota = BancoDeDados.NotasFiscais.Where(x => x.numeroNF == novaNf.numeroNF && x.fornecedorNF == novaNf.fornecedorNF).FirstOrDefault();
                    nota.skuNF = novaNf.skuNF;
                    nota.volumesNF = novaNf.volumesNF;
                    BancoDeDados.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var olho = ex;
            }
        }

        public void InserirCteNaNf(string numNF, int numeroCte)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    NotasFiscais nfAtual = BancoDeDados.NotasFiscais.FirstOrDefault(nf => nf.numeroNF == numNF);
                    var ctes = GetNovoCtePorNum(numeroCte);
                    nfAtual.CteNovoNF = ctes.Where(x => x.notasCte.Contains(numNF)).Select(x => x.idCte).LastOrDefault();
                    BancoDeDados.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion Documentos

        #region Pessoas

        public bool EditarFuncionario(Funcionarios novoFunc)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Funcionarios funcAtual = BancoDeDados.Funcionarios.Single(f => f.idFunc == novoFunc.idFunc);
                    funcAtual.nomeFunc = novoFunc.nomeFunc;
                    funcAtual.matriculaFunc = novoFunc.matriculaFunc;
                    funcAtual.tipoFunc = novoFunc.tipoFunc;
                    funcAtual.senhaFunc = novoFunc.senhaFunc;
                    funcAtual.ativoFunc = novoFunc.ativoFunc;
                    BancoDeDados.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void AlterarOcupacaoFuncionario(int idFuncionario, bool ocupado)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Funcionarios funcAtual = BancoDeDados.Funcionarios.Single(f => f.idFunc == idFuncionario);
                    funcAtual.ocupadoFunc = ocupado;
                    BancoDeDados.SaveChanges();
                }
            }
            catch
            {
            }
        }

        #endregion Pessoas

        #region Veiculos

        public void RegistrarSaída(AcessosPortaria acesso)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var antigo = (from AcessosPortaria in BancoDeDados.AcessosPortaria where AcessosPortaria.idAcesso == acesso.idAcesso select AcessosPortaria).FirstOrDefault();
                    antigo.SaidaAcesso = DateTime.Now;
                    antigo.KmAcesso = acesso.KmAcesso;
                    antigo.Placa2Acesso = acesso.Placa2Acesso;
                    antigo.PorteiroSaida = acesso.PorteiroSaida;
                    BancoDeDados.SaveChanges();
                }
            }
            catch (Exception erro)
            {
            }
        }

        public void RegistrarDoca(int IdAcesso, int Doca)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var acesso = (from AcessosPortaria in BancoDeDados.AcessosPortaria where AcessosPortaria.idAcesso == IdAcesso select AcessosPortaria).FirstOrDefault();
                    acesso.DocaAcesso = Doca;
                    BancoDeDados.SaveChanges();
                }
            }
            catch (Exception erro)
            {
            }
        }

        #endregion Veiculos

        #region Tarefas

        public bool FinalizarTarefa(int idTarefa, int quantPalet, int totalPalet)
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

                    foreach (Func_Tarefa f in tarefaAtual.Func_Tarefa)
                    {
                        AlterarOcupacaoFuncionario(f.Funcionario, false);
                    }
                }
                return true;
            }
            catch (Exception erro)
            {
                return false;
            }
        }

        public void InserirPontuacao(int idTarefa, float pontos)
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
            }
            catch
            {
            }
        }
        
        #endregion Tarefas

        #endregion Alteracoes

        #region NotDatabase

        private List<TarefaModelo> TarefaModeloParse(List<Tarefas> tarefas)
        {
            List<TarefaModelo> modelos = new List<TarefaModelo>();
            foreach (Tarefas tar in tarefas)
            {
                if (tar != null)
                {
                    modelos.Add(TarefaModeloParse(tar));
                }
            }
            return modelos;
        }

        private TarefaModelo TarefaModeloParse(Tarefas tarefas)
        {
            TarefaModelo aux;
            Manifestos m;
            aux = new TarefaModelo(tarefas);

            if (tarefas.tipoTarefa == "2")
            {
                aux.documentoTarefa = GetCtePorID(aux.documentoTarefa).numeroCte;
                aux.valores(GetSkuCte(tarefas.documentoTarefa), GetVolumesCte(tarefas.documentoTarefa));
                aux.fornecedor = GetFornecedorCte(tarefas.documentoTarefa);
                aux.cliente = GetListaManifestosCte(tarefas.documentoTarefa);
            }
            else
            {
                m = GetManifestoPorNumero(tarefas.documentoTarefa);
                aux.valores(0, m.VolumesManifesto);
            }
            aux.nomesFuncionarios = GetNomesFuncTarefa(tarefas.idTarefa);

            return aux;
        }

        public List<ItemRanking> RankingFuncionarios(List<ItemRanking> tarefasPeriodo)
        {
            var lista = from t in tarefasPeriodo
                        group t by new
                        {
                            t.NomesFuncionarios
                        } into g
                        select new
                        {
                            Sum = g.Sum(p => p.Pontuacao),
                            g.Key.NomesFuncionarios,
                        };

            List<ItemRanking> Rank = new List<ItemRanking>();
            int cont = 0;
            int erros = 0;

            foreach (var item in lista)
            {
                erros = tarefasPeriodo.Where(x => x.NomesFuncionarios == item.NomesFuncionarios && x.Pontuacao == 0).Count();
                cont = tarefasPeriodo.Where(x => x.NomesFuncionarios == item.NomesFuncionarios).Count();
                Rank.Add(new ItemRanking()
                {
                    Pontuacao = Math.Round(item.Sum, 2),
                    NomesFuncionarios = item.NomesFuncionarios,
                    QuantidadeTarefas = cont,
                    Erros = erros
                });
            }

            Rank = Rank.OrderByDescending(i => i.Pontuacao).ToList();

            return Rank;
        }

        private List<ItemRelatorio> ConsolidarRelatorio(List<RelatorioNovoConferencias> conferencias, List<RelatorioNaoConferencia> outros)
        {
            List<ItemRelatorio> lista = new List<ItemRelatorio>();
           
            foreach (var tarefa in conferencias)
            {
                var x = lista.Where(id => id.idTarefa == tarefa.idTarefa && !(id.nomesFunc.Contains(tarefa.nomeFunc))).FirstOrDefault();
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
                    lista.Add(new ItemRelatorio(tarefa)
                    {
                        fornecedor = GetFornecedorManifesto(tarefa.documentoTarefa),
                        ctesNoManifesto = GetManifestoPorNumero(tarefa.documentoTarefa).quantCtesManifesto
                    });
                }
                else
                {
                    int index = lista.IndexOf(x);
                    lista[index].nomesFunc = lista[index].nomesFunc + "/" + tarefa.nomeFunc;
                }
            }
            return lista;
        }

        #endregion


        public string GetPastaNFs()
        {
            return PastasXml.Default.PastaNFs;
        }

        public string GetPastaPreManifestos()
        {
            return PastasXml.Default.PastaPreManifestos;
        }

        public string GetPastaManifestos()
        {
            return PastasXml.Default.PastaManifestos;
        }

        public void SetPastasNF(string caminho)
        {
            PastasXml.Default.PastaNFs = caminho;
            PastasXml.Default.Save();
        }

        public void SetPastasManifesto(string caminho)
        {
            PastasXml.Default.PastaManifestos = caminho;
            PastasXml.Default.Save();
        }

        public void SetPastasPreManifesto(string caminho)
        {
            PastasXml.Default.PastaPreManifestos = caminho;
            PastasXml.Default.Save();
        }

        /*  TELA DE DIVERGENCIAS ANTIGAS
         *  public bool CadastrarDivergencia(List<TarefaModelo> listaDivergencia)
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

            public List<TarefaModelo> GetTarefasDivergencia(int tipo, int documento)
            {
                List<TarefaModelo> listaModelo = new List<TarefaModelo>();
                List<Tarefas> lista = new List<Tarefas>();

                try
                {
                    using (var BancoDeDados = new produsisBDEntities())
                    {
                        if (tipo == 2)
                        {
                            try
                            {
                                var listaCtes = GetNovoCtePorNum(documento);
                                //Se o tipo desejado for Conferência, retorna todas as tarefas com ctes relacionados com o manifesto do cte informado
                                foreach (var item in listaCtes)
                                {
                                    int manif = BancoDeDados.Cte_Manifesto.Where(c => c.CteNovo == item.idCte).Select(m => m.Manifesto).FirstOrDefault();
                                    var ctes = BancoDeDados.Cte_Manifesto.Where(m => m.Manifesto == manif).Select(c => c.CteNovo).ToList(); //lista dos ids
                                    foreach (int cte in ctes)
                                    {
                                        lista.Add(BancoDeDados.Tarefas.Where(c => c.documentoTarefa == cte).FirstOrDefault());
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                            }
                        }
                        else
                        { //Se for outro tipo, retorna só aquele tipo e manifesto
                            lista.Add(BancoDeDados.Tarefas.Where(c => c.documentoTarefa == documento && c.tipoTarefa == tipo.ToString()).FirstOrDefault());
                        }
                        listaModelo = TarefaModeloParse(lista);
                    }
                }
                catch (Exception e)
                {
                    var whatHapened = e;
                }
                return listaModelo;
            }
      */

    }
}