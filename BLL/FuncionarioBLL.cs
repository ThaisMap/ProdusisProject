using DAL;
using ProdusisBD;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class FuncionarioBLL
    {
        private FuncionariosBD f = new FuncionariosBD();

        public List<string> carregaFuncionarios()
        {
            return f.getListaNomes();
        }

        public List<String> carregaFuncionariosLivres()
        {
            return f.funcionariosLivres();
        }

        public bool validarSenha(string matricula, string senha)
        {
            return f.verificaSenha(matricula, senha);
        }

        public bool salvarNovo(Funcionarios novoFunc)
        {
            return f.cadastrar(novoFunc);
        }

        public bool editar(Funcionarios novosDados)
        {
            return f.editar(novosDados);
        }

        public bool nomeCadastrado(string nome)
        {
            if (f.getFuncPorNome(nome) == null)
            {
                return false;
            }
            return true;
        }

        public Funcionarios dadosFuncionario(string nome)
        {
            return f.getFuncPorNome(nome);
        }

        public string tipoFuncionario(string matricula)
        {
            return f.getTipoFuncionario(matricula);
        }

        public bool validarUsuario(string matricula)
        {
            return f.verificaUsuarioCadastrado(matricula);
        }
      
    }
}