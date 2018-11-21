using ProdusisBD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public class FuncionariosBD
    {
        /// <summary>
        /// Insere um registro de funcionario no banco de dados
        /// </summary>
        /// <param name="novoFunc">Dados do novo registro</param>
        /// <returns>True se o comando foi executado sem erros, False se houve algum erro</returns>
        public bool cadastrar(Funcionarios novoFunc)
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

        public bool cadastrarObservacao(Observacoes novaObs)
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
                var olho = e;
                return false;
            }
        }

        public void deletarObservacao(int idObservacao)
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

        public List<Observacoes> getObservacoes(DateTime? inicio, DateTime fim)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var lista = (from Observacoes in BancoDeDados.Observacoes where Observacoes.DataObs <= fim.Date select Observacoes);
                    if (inicio != null)
                        lista = lista.Where(d => d.DataObs >= inicio);

                    foreach (var item in lista)
                    {
                        item.NomeFunc = item.Funcionarios.nomeFunc;
                    }

                    return lista.ToList();
                };
            }
            catch (Exception e)
            {
                return new List<Observacoes>();
            }
        }

        /// <summary>
        /// Altera um registro de funcionario no banco de dados
        /// </summary>
        /// <param name="novoFunc">Dados que serão salvos no banco</param>
        /// <returns>True se o comando foi executado sem erros, False se houve algum erro</returns>
        public bool editar(Funcionarios novoFunc)
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
                var whatHappened = e;
                return false;
            }
        }

        /// <summary>
        /// Edita o estado de oucupação de um funcionário
        /// </summary>
        /// <param name="idFuncionario">Funcionário alterado</param>
        /// <param name="ocupado">Valor a ser registrado no banco</param>
        /// <returns>True se o comando foi executado sem erros, False se houve algum erro</returns>
        public bool editarOcupacaoFuncionario(int idFuncionario, bool ocupado)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    Funcionarios funcAtual = BancoDeDados.Funcionarios.Single(f => f.idFunc == idFuncionario);
                    funcAtual.ocupadoFunc = ocupado;
                    BancoDeDados.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retorna a lista de operadores ativos com status livre
        /// </summary>
        public List<Funcionarios> funcionariosLivres(string tipo)
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

        public List<string> conferentesLivres(string tipo)
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

        /// <summary>
        /// Retorna um Funcionário com base no id
        /// </summary>
        /// <param name="id">Parâmetro de consulta</param>
        public Funcionarios getFuncPorId(int id)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return BancoDeDados.Funcionarios.Where(f => f.idFunc == id).FirstOrDefault();
                }
            }
            catch
            {
                return new Funcionarios();
            }
        }

        public Funcionarios getFuncPorMatricula(string Matricula)
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

        /// <summary>
        /// Retorna o tipo do Funcionario
        /// </summary>
        /// <returns>0 - Administrativo, 1 - Operacional</returns>
        public string getTipoFuncionario(string matricula)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return BancoDeDados.Funcionarios.Where(f => f.matriculaFunc == matricula).Select(f => f.tipoFunc).FirstOrDefault();
                }
            }
            catch
            {
                return "1";
            }
        }

        /// <summary>
        ///  Retorna um Funcionário com base no nome (o primeiro encontrado no banco)
        /// </summary>
        /// <param name="nomeUsuario">Parâmetro de consulta</param>
        public Funcionarios getFuncPorNome(string nomeUsuario)
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

        /// <summary>
        /// Retorna uma lista com os nomes de todos os funcionários cadastrados
        /// </summary>
        public List<string> getListaNomes()
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

        public List<Funcionarios> getListaFuncionariosAtivos()
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

        /// <summary>
        /// Verifica se a senha do Funcionário indicado coincide com a registrada no banco de dados
        /// </summary>
        /// <param name="matricula">Parâmetro de busca</param>
        /// <param name="senha">Senha para comparação</param>
        /// <returns>True se a senha coincidir, False se o funcionário nao estiver cadastrado, ou se a senha nao coincidir ou se</returns>
        public bool verificaSenha(string matricula, string senha)
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

        /// <summary>
        /// Verifica se o Funcionário indicado está cadastrado no banco de dados
        /// </summary>
        ///
        /// <param name="matricula">Martricula do Funcionário</param>
        /// <returns>True se estiver cadastrado, False se não estiver ou se ocorrer um erro</returns>
        public bool verificaUsuarioCadastrado(string matricula)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    var cadastrado = (from Funcionarios in BancoDeDados.Funcionarios
                                      where Funcionarios.matriculaFunc == matricula
                                      select Funcionarios.tipoFunc).FirstOrDefault();
                    if (cadastrado != null)
                    {
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception e)
            {
                var erro = e;
                return false;
            }
        }

        public bool salvaEquipe(List<string> equipe, int? numEquipe)
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
                var erro = e;
                return false;
            }
        }
    }
}