using System;

namespace ProdusisBD
{
    public class ItemRanking
    {
        public double Pontuacao { get; set; }
        public string NomesFuncionarios { get; set; }
        public int QuantidadeTarefas { get; set; }
        public int Erros { get; set; }

        public ItemRanking(double media, string nomes, int quant)
        {
            Pontuacao = Math.Round(media, 2);
            NomesFuncionarios = nomes;
            QuantidadeTarefas = quant;
        }

        public ItemRanking()
        {
                
        }
    }
}