using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace ProdusisBD
{
    public class Testes
    {
        public static string testeConn()
        {
            string resultado = "";

            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; User ID=ThaisMap; Password=deserto; Database=produsisBD");
            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand("Select \"nomeFunc\" from Funcionarios where \"idFunc\" = 1", conn);
            // you have to either change the columns names to lower case, or quote everything
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
               resultado = dr.GetString(0);
            }
            else
            {
                resultado = "Nope";
            }

            dr.Close();
            conn.Close();

            return resultado;
        }
    }
}
