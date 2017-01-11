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
            catch
            {
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
        public List<String> funcionariosLivres()
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Funcionarios in BancoDeDados.Funcionarios where Funcionarios.ativoFunc == true where Funcionarios.ocupadoFunc == false where Funcionarios.tipoFunc == "1" orderby Funcionarios.nomeFunc select Funcionarios.nomeFunc).ToList();
                }
            }
            catch
            {
                return new List<String>();
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
            catch
            {
                return new List<string>();
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
                    if (cadastrado != null && cadastrado != "1")
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
    }
}