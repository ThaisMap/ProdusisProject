using System;

namespace ProdusisBD
{
    public class ItemRanking
    {
        public double mediaPorHora { get; set; }
        public string nomesFuncionarios { get; set; }
        public int quantidadeTarefas { get; set; }
        public string observacoes { get; set; }

        public ItemRanking(double media, string nomes, int quant)
        {
            mediaPorHora = Math.Round(media, 2);
            nomesFuncionarios = nomes;
            quantidadeTarefas = quant;
        }
    }
}