using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProdusisBD;

namespace DAL
{
    class TarefaPontuacao
    {
        public string nomesFuncionarios { get; set; }
        public int skus { get; set; }
        public int volumes { get; set; }
        public int? quantPaletizado { get; private set; }
        public int? totalPaletes { get; private set; }
        

        public TarefaPontuacao(Tarefas t)
        {
            quantPaletizado = t.quantPaletizado == null ? 1 : t.quantPaletizado;
            totalPaletes = t.totalPaletes == null ? 1 : t.totalPaletes;
        }
    }
}
