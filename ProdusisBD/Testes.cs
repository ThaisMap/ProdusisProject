using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ProdusisBD;


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
                              where func.idFunc == 0
                              select func.nomeFunc).FirstOrDefault();
                
            }
            

          
            return resultado;
        }
    }
}
