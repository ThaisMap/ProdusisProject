using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdusisBD
{
    public class ItemRelatorio
    {
        public int idTarefa { get; set; }
        public int documentoTarefa { get; set; }
        public string tipoTarefa { get; set; }
        public DateTime inicioTarefa { get; set; }
        public Nullable<DateTime> fimTarefa { get; set; }
        public string divergenciaTarefa { get; set; }
        public string nomesFunc { get; set; }
        public int volumes { get; set; }
        public int sku { get; set; }
        public int? quantPaletizado { get; set; }
        public int? totalPaletes { get; set; }
        public string fornecedor { get; set; }
        public string tempoGasto { get; set; }
        public string dataInicio { get; set; }
        public string horaInicio { get; set; }
        public string dataFim { get; set; }
        public string horaFim { get; set; }
        public double pontos { get; set; }
        public int ctesNoManifesto { get; set; }

        public ItemRelatorio(RelatorioNovoConferencias r)
        {
            idTarefa = r.idTarefa;
            documentoTarefa = r.numeroCte;
            tipoTarefa = tipoExtenso(r.tipoTarefa);
            inicioTarefa = r.inicioTarefa;
            fimTarefa = r.fimTarefa;
            nomesFunc = r.nomeFunc;
            volumes = r.volumesNF;
            sku = r.skuNF;
            fornecedor = r.fornecedorNF;
            divergenciaTarefa = r.divergenciaTarefa;
            divergenciaTarefa = divergencia();
            preencheDatas();
            atualizaTempoGasto();
            ctesNoManifesto = 0;
        }

        public ItemRelatorio(RelatorioNaoConferencia r)
        {
            idTarefa = r.idTarefa;
            documentoTarefa = r.documentoTarefa;
            tipoTarefa = tipoExtenso(r.tipoTarefa);
            inicioTarefa = r.inicioTarefa;
            fimTarefa = r.fimTarefa;
            nomesFunc = r.nomeFunc;
            volumes = r.VolumesManifesto;
            sku = 0;
            fornecedor = "";
            quantPaletizado = r.quantPaletizado;
            totalPaletes = r.totalPaletes;
            divergenciaTarefa = r.divergenciaTarefa;
            divergenciaTarefa = divergencia();
            preencheDatas();
            atualizaTempoGasto();
            ctesNoManifesto = 1;
        }

        public ItemRelatorio()
        {
            pontos = 0;
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

        public void preencheDatas()
        {
            dataInicio = inicioTarefa.Date.ToString("dd\\/MM\\/yyyy");
            horaInicio = inicioTarefa.ToString("HH\\:mm\\:ss");

            if (fimTarefa != null)
            {
                dataFim = ((DateTime)fimTarefa).ToString("dd-MM-yyyy");
                horaFim = ((DateTime)fimTarefa).ToString("HH\\:mm\\:ss");
            }
        }

        public string divergencia()
        {
            string[] d = divergenciaTarefa.Split(';');
            string retorno = "";
            if (d[0] != "-")
            {
                retorno = "Falta código(s): " + d[0] + " qtde(s): " + d[1];
            }
            if (d[2] != "-")
            {
                if (retorno == "")
                    retorno = "Sobra código(s): " + d[2] + " qtde(s): " + d[3];
                else
                    retorno += " - Sobra código(s): " + d[2] + " qtde(s): " + d[3];
            }
            if (d[4] != "-")
            {
                if (retorno == "")
                    retorno = "Avaria código(s): " + d[4] + " qtde(s): " + d[5];
                else
                    retorno += " - Avaria código(s): " + d[4] + " qtde(s): " + d[5];
            }
            if (retorno == "")
                return "Nenhuma";

            return retorno;
        }
        
        /// <summary>
        /// Calcula pontuação para ranking.  
        /// </summary>
         public void atualizaPontuação()
        {
            if (tipoTarefa == "Conferência")
            {
                pontos = sku * 7 + volumes * 0.4;
            }
            else if (tipoTarefa.Contains("Descarga") || tipoTarefa.Contains("Carregamento"))
            {
                if (quantPaletizado > totalPaletes)
                    quantPaletizado = totalPaletes;
                double porcentagemPaletizado = (double)quantPaletizado / (double)totalPaletes;
                pontos = volumes * porcentagemPaletizado;
                pontos += volumes * (1 - porcentagemPaletizado) * 3;
            }
            else // regra para separação e movimentacao de empilhadeira
            {
                pontos = (double)totalPaletes;
            }
            if (divergenciaTarefa != "Nenhuma" && divergenciaTarefa != "-;0;-;0;-;0")
            {
                pontos = 0;
            }
            if (nomesFunc.Contains("/"))
            {
                int div = nomesFunc.Count()-nomesFunc.Replace("/", string.Empty).Count()+1;
                pontos = pontos / div;
            }
            
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
                    return "Sep. para carregar";
                case "4":
                    return "Carregamento";
                case "5":
                    return "Empilhadeira";
                default:
                    return "Carregamento Paletizado";
            }
        }
    }
}
