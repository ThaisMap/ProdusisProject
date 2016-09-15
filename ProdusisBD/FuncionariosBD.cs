using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdusisBD
{
    class FuncionariosBD
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
                    //Clientes clienteAtual = BancoDeDados.Clientes.Single(l => l.Id == novoCliente.Id);
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
        /// Retorna a lista de operadores com status livre
        /// </summary>
        public List<Funcionarios> funcionariosLivres()
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Funcionarios in BancoDeDados.Funcionarios where Funcionarios.ocupadoFunc == false where Funcionarios.tipoFunc == "2" select Funcionarios).ToList();
                }
            }
            catch
            {
                return new List<Funcionarios>();
            }
        }

        public List<Funcionarios> getFuncionariosAtivos()
        {
            return new List<Funcionarios>();
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
                    return (from Funcionarios in BancoDeDados.Funcionarios where Funcionarios.idFunc == id select Funcionarios).FirstOrDefault();
                }
            }
            catch
            {

                return new Funcionarios();
            }            
        }

        /// <summary>
        ///  Retorna um Funcionário com base na matrícula
        /// </summary>
        /// <param name="matricula">Parâmetro de consulta</param>
        public Funcionarios getFuncPorMatricula(string matricula)
        {
            try
            {
                using (var BancoDeDados = new produsisBDEntities())
                {
                    return (from Funcionarios in BancoDeDados.Funcionarios where Funcionarios.matriculaFunc == matricula select Funcionarios).FirstOrDefault();
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
                    return (from Funcionarios in BancoDeDados.Funcionarios select Funcionarios.nomeFunc).ToList();
                }
            }
            catch
            {
                return new List<string>();
            }
        }

        public int getTipoFuncionario(string nomeUsuario)
        {

            return 1;
        }

        public bool verificaSenha(string senha)
        {
            try
            {

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool verificaUsuarioCadastrado(string nomeUsuario)
        {
            try
            {

                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
