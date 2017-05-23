using System;
using System.Globalization;


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
            tipoTarefa = tipoExtenso(tarefa.tipoTarefa);
            preencheDatas();
            atualizaTempoGasto();
            divergenciaTarefa = tarefa.divergenciaTarefa;
        }

        private string tipoExtenso(string tipo)
        {
            switch (tipo)
            {
                case "0":
                    return "Descarga";
                case "1":
                    return "Separação";
                case "2":
                    return "Conferência";
                case "3":
                    return "Sep. para carregar";
                default:
                    return "Carregamento";
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
        public int pontos { get; set; }
        public double pontosPorHora { get; set; }


        public void valores(int sku, int volume)
        {
            skus = sku;
            volumes = volume;
        }

        public string divergencia(string div)
        {
            string[] d = div.Split(';');
            string retorno = "";
            if (d[0] != "-")
            {
                retorno = "Falta código(s): " + d[0] + " qtde(s): " + d[1];
            }
            if (d[2] != "-")
            {
                if (retorno == "")
                    retorno = "Sobra código(s): " + d[0] + " qtde(s): " + d[1];
                else
                    retorno += " - Sobra código(s): " + d[0] + " qtde(s): " + d[1];
            }
            if (d[2] != "-")
            {
                if (retorno == "")
                    retorno = "Avaria código(s): " + d[0] + " qtde(s): " + d[1];
                else
                    retorno += " - Avaria código(s): " + d[0] + " qtde(s): " + d[1];
            }
            if (retorno == "")
                return "Nenhuma";
            return retorno;
        }
               
        

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
            tempoGasto = (tempo.Days * 24 + tempo.Hours).ToString("00") + ":" + tempo.Minutes.ToString("00") + ":" + tempo.Seconds.ToString("00");
        }

        /// <summary>
        /// Aqui você altera o calculo dos pontos. Peso atual do sku: 5
        ///
        /// </summary>
        public void atualizaPontuação()
        {
            pontos = skus * 5 + volumes;
            double segundos = ((DateTime)fimTarefa - inicioTarefa).TotalSeconds;
            pontosPorHora = pontos / segundos * 3600;
        }

        public void preencheDatas()
        {
            CultureInfo brasil = new CultureInfo("pt-BR");
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