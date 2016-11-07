using ProdusisBD;
using System.Data;
using System.Linq;

namespace DAL
{
    public class Testes
    {
        public static string testeConn()
        {
            string resultado = "";

            using (produsisBDEntities pbd = new produsisBDEntities())
            {
                resultado = (from func in pbd.Funcionarios
                             where func.idFunc == 1
                             select func.nomeFunc).FirstOrDefault();
            }

            return resultado;
        }
    }
}