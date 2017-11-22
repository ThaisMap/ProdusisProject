using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdusisBD
{
    class ItemRelatorio
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
        public string fonecedor { get; set; }
        public string tempoGasto { get; set; }
        public string dataInicio { get; private set; }
        public string horaInicio { get; private set; }
        public string dataFim { get; private set; }
        public string horaFim { get; private set; }
        public double pontos { get; private set; }

        public ItemRelatorio(RelatorioConferencias r)
        {
            idTarefa = r.idTarefa;
            documentoTarefa = r.documentoTarefa;
            tipoTarefa = r.tipoTarefa;
            inicioTarefa = r.inicioTarefa;
            fimTarefa = r.fimTarefa;
            divergenciaTarefa = r.divergenciaTarefa;
            nomesFunc = r.nomeFunc;
            volumes = r.volumesNF;
            sku = r.skuNF;
            fonecedor = r.fonecedorNF;
        }

        public ItemRelatorio(RelatorioNaoConferencia r)
        {
            idTarefa = r.idTarefa;
            documentoTarefa = r.documentoTarefa;
            tipoTarefa = r.tipoTarefa;
            inicioTarefa = r.inicioTarefa;
            fimTarefa = r.fimTarefa;
            divergenciaTarefa = r.divergenciaTarefa;
            nomesFunc = r.nomeFunc;
            volumes = r.VolumesManifesto;
            sku = r.skusManifesto;
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

        /// <summary>
        /// Calcula pontuação para ranking.  
        /// </summary>
  /*      public void atualizaPontuação()
        {
            if (tipoTarefa == "Conferência")
            {
                pontos = sku * 7 + volumes * 0.4;
            }
            else if (tipoTarefa.Contains("Descarga") || tipoTarefa.Contains("Carregamento"))
            {
                pontos = volumes * (float)porcentagemPaletizado * 3;
                pontos += volumes * (1 - (float)porcentagemPaletizado);
            }
            if (divergenciaTarefa != "Nenhuma" && divergenciaTarefa != "-;0;-;0;-;0")
            {
                pontos = 0;
            }
        }*/
    }
}
