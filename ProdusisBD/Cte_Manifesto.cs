//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProdusisBD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cte_Manifesto
    {
        public int idCteManifesto { get; set; }
        public int Manifesto { get; set; }
        public int CteNovo { get; set; }

        public Cte_Manifesto(int nManifesto, int idCte)
        {
            Manifesto = nManifesto;
            CteNovo = idCte;
        }

        public Cte_Manifesto()
        {

        }

        public virtual Cte Cte { get; set; }
        public virtual Manifestos Manifestos { get; set; }
    }
}
