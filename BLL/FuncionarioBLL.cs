using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProdusisBD;
using DAL;

namespace BLL
{
    public class FuncionarioBLL
    {
        private FuncionariosBD f = new FuncionariosBD();
        
        public List<string> carregaFuncionarios()
        {
            return f.getNomeFuncionarios();
        }

        public bool salvarNovo(Funcionarios novoFunc)
        {
            return f.cadastrar(novoFunc);
        }

        public bool editar(Funcionarios novosDados)
        {
            return f.editar(novosDados);
        }

    }
}
