using DAL;

namespace BLL
{
    public class LoginBLL
    {
        private FuncionariosBD f = new FuncionariosBD();

        /// <summary>
        /// Verifica se o funcionario está cadastrado
        /// </summary>
        /// <returns>True se o funcionário estiver cadastrado, False se nao estiver</returns>
        public bool validarUsuario(string matricula)
        {
            return f.verificaUsuarioCadastrado(matricula);
        }

        /// <summary>
        /// valida a senha para aquela matricula
        /// </summary>
        /// <returns>True se a senha for a mesma do cadastro, false se não for</returns>
        public bool validarSenha(string matricula, string senha)
        {
            return f.verificaSenha(matricula, senha);
        }

        /// <summary>
        /// Carrega o funcionário como logado no sistema
        /// </summary>
        /// <param name="matricula"></param>
        public void logarFuncionario(string matricula)
        {
            Sessao.funcionarioLogado = f.getFuncPorMatricula(matricula);
        }
    }
}