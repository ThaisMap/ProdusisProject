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

        public Cte_Manifesto(int nCte, int nManifesto)
        {
            Cte = nCte;
            Manifesto = nManifesto;
        }

        public Cte_Manifesto()
        {

        }

        public int idCteManifesto { get; set; }
        public int Cte { get; set; }
        public int Manifesto { get; set; }
    
        public virtual Ctes Ctes { get; set; }
        public virtual Manifestos Manifestos { get; set; }
    }
}
