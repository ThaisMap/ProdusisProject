using System;

namespace ProdusisBD
{
    public class ItemRanking
    {
        public double Pontuacao { get; set; }
        public string NomeFuncionario { get; set; }
        public int QuantidadeTarefas { get; set; }
        public int Erros { get; set; }
        public string TipoTarefa { get; set; }
        public string Matricula { get; set; }

        public ItemRanking(double pontos, string nome, int quant, int erros)
        {
            Pontuacao = Math.Round(pontos, 2);
            NomeFuncionario = nome;
            QuantidadeTarefas = quant;
            Erros = erros;
        }

        public ItemRanking()
        {
                
        }
    }
}