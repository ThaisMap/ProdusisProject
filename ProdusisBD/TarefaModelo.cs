using System;

namespace ProdusisBD
{
    public class TarefaModelo : Tarefas
    {
        public TarefaModelo(Tarefas tarefa)
        {
            inicioTarefa = tarefa.inicioTarefa;
            fimTarefa = tarefa.fimTarefa;
            idTarefa = tarefa.idTarefa;
            documentoTarefa = tarefa.documentoTarefa;
            tipoTarefa = tarefa.tipoTarefa;
            preencheDatas();
            atualizaTempoGasto();
        }

        public string dataInicio { get; set; }
        public string horaInicio { get; set; }
        public string dataFim { get; set; }
        public string horaFim { get; set; }
        public string nomesFuncionarios { get; set; }

        public void atualizaTempoGasto()
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
            tempoGasto = (tempo.Days * 24 + tempo.Hours) + ":" + tempo.Minutes + ":" + tempo.Seconds;
        }

        public void preencheDatas()
        {
            dataInicio = inicioTarefa.Date.ToString("dd\\/MM\\/yyyy");
            horaInicio = inicioTarefa.ToString("hh\\:mm\\:ss");

            if (fimTarefa != null)
            {
                dataFim = ((DateTime)fimTarefa).ToString("dd\\-mm\\-yyyy");
                horaInicio = ((DateTime)fimTarefa).ToString("hh\\:mm\\:ss");
            }
        }
    }
}