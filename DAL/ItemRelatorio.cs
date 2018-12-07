using System;
using ProdusisBD;
using System.Linq;

namespace DAL
{
    public class ItemRelatorio
    {
        public int idTarefa { get; set; }
        public int documentoTarefa { get; set; }
        public string tipoTarefa { get; set; }
        public DateTime inicioTarefa { get; set; }
        public DateTime? fimTarefa { get; set; }
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
            tipoTarefa = TipoExtenso(r.tipoTarefa);
            inicioTarefa = r.inicioTarefa;
            fimTarefa = r.fimTarefa;
            nomesFunc = r.nomeFunc;
            volumes = (int)r.Volumes;
            sku = (int)r.SKU;
            fornecedor = r.fornecedorNF;
            divergenciaTarefa = r.divergenciaTarefa;
            divergenciaTarefa = Divergencia();
            PreencheDatas();
            AtualizaTempoGasto();
            ctesNoManifesto = 0;
        }

        public ItemRelatorio(RelatorioNaoConferencia r)
        {
            idTarefa = r.idTarefa;
            documentoTarefa = r.documentoTarefa;
            tipoTarefa = TipoExtenso(r.tipoTarefa);
            inicioTarefa = r.inicioTarefa;
            fimTarefa = r.fimTarefa;
            nomesFunc = r.nomeFunc;
            volumes = r.VolumesManifesto;
            sku = 0;
            fornecedor = "";
            quantPaletizado = r.quantPaletizado;
            totalPaletes = r.totalPaletes;
            divergenciaTarefa = r.divergenciaTarefa;
            divergenciaTarefa = Divergencia();
            PreencheDatas();
            AtualizaTempoGasto();
            ctesNoManifesto = 1;
        }

        public ItemRelatorio()
        {
            pontos = 0;
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

        public string Divergencia()
        {
            AcessoBD abd = new AcessoBD();
            return abd.GetDadosDivergencia(idTarefa);                    
        }

        public void AtualizaPontuação()
        {
            try
            {
                if (divergenciaTarefa != "Nenhuma")
                {
                    pontos = 0;
                }
                else
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

                    if (nomesFunc.Contains("/"))
                    {
                        int div = nomesFunc.Count (x => x == '/') + 1;
                        pontos = pontos / div;
                    }
                }
            }
            catch 
            {
            }
        }

        private string TipoExtenso(string tipo)
        {
            switch (tipo)
            {
                case "0":
                case "1":
                    return "Descarga";

                case "2":
                    return "Conferência";

                case "3":
                    return "Movim. de Paletes";

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