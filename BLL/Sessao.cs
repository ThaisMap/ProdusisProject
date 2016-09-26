using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProdusisBD;

namespace BLL
{
    public partial class Sessao
    {
        public static Funcionarios funcionarioLogado = new Funcionarios();
    }

    public enum tiposDocumento
    {
        Manifesto,
        Cte, 
        NotaFiscal
    }

    public enum tipoFuncionario
    {
        Administrativo, 
        Externo,
        Operador
    }
}
