using System;
using ProdusisBD;

namespace DAL
{
    public class TarefaModelo : Tarefas
    {
        public TarefaModelo(Tarefas tarefa)
        {
            inicioTarefa = tarefa.inicioTarefa;
            fimTarefa = tarefa.fimTarefa;
            idTarefa = tarefa.idTarefa;
            documentoTarefa = tarefa.documentoTarefa;
            tipoTarefa = tipoExtenso(tarefa.tipoTarefa);
            PreencheDatas();
            AtualizaTempoGasto();
            divergenciaTarefa = tarefa.divergenciaTarefa;
            totalPaletes = tarefa.totalPaletes;
        }

        private string tipoExtenso(string tipo)
        {
            switch (tipo)
            {
                case "0":
                case "1":
                    return "Descarga";

                case "2":
                    return "Conferência";

                case "3":
                    return "Mov. de paletes";

                case "4":
                    return "Carregamento";

                case "5":
                    return "Empilhadeira";

                default:
                    return "Carregamento Paletizado";
            }
        }

        public string dataInicio { get; set; }
        public string horaInicio { get; set; }
        public string dataFim { get; set; }
        public string horaFim { get; set; }
        public string nomesFuncionarios { get; set; }
        public int skus { get; set; }
        public int volumes { get; set; }
        public string fornecedor { get; set; }
        public string cliente { get; set; }
        public double pontos { get; set; }
        public string tempoGasto { get; set; }

        public void IncluirValores(int sku, int volume)
        {
            skus = sku;
            volumes = volume;
        }

        /// <summary>
        /// Retorna uma string referente às divergencias registradas
        /// </summary>
        public string Divergencia()
        {
            AcessoBD abd = new AcessoBD();
            return abd.GetDadosDivergencia(idTarefa); 
        }

        public void AtualizaTempoGasto()
        {
            TimeSpan tempo;
            if (fimTarefa == null)
            {
                tempo = DateTime.Now - inicioTarefa;
            }
            else
            {
                tempo = (DateTime)fimTarefa - inicioTarefa;
            }
            tempoGasto = (tempo.Days * 24 + tempo.Hours).ToString("00") + ":" + tempo.Minutes.ToString("00") + ":" + tempo.Seconds.ToString("00");
        }

        public void PreencheDatas()
        {
            dataInicio = inicioTarefa.Date.ToString("dd\\/MM\\/yyyy");
            horaInicio = inicioTarefa.ToString("HH\\:mm\\:ss");

            if (fimTarefa != null)
            {
                dataFim = ((DateTime)fimTarefa).ToString("dd-MM-yyyy");
                horaFim = ((DateTime)fimTarefa).ToString("HH\\:mm\\:ss");
            }
        }
    }
}