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
    
    public partial class Cte
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cte()
        {
            this.Cte_Manifesto = new HashSet<Cte_Manifesto>();
            this.NotasFiscais = new HashSet<NotasFiscais>();
        }

        public Cte(int nCte, string nfs)
        {
            numeroCte = nCte;
            notasCte = nfs;
            this.Cte_Manifesto = new HashSet<Cte_Manifesto>();
            this.NotasFiscais = new HashSet<NotasFiscais>();
        }

        public int idCte { get; set; }
        public int numeroCte { get; set; }
        public string notasCte { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cte_Manifesto> Cte_Manifesto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotasFiscais> NotasFiscais { get; set; }
    }
}