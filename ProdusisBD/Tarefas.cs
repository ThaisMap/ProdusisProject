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
    
    public partial class Tarefas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tarefas()
        {
            this.Func_Tarefa = new HashSet<Func_Tarefa>();
        }
    
        public int idTarefa { get; set; }
        public System.DateTime inicioTarefa { get; set; }
        public Nullable<System.DateTime> fimTarefa { get; set; }
        public Nullable<int> documentoTarefa { get; set; }
        public string tipoTarefa { get; set; }
    
        public virtual Ctes Ctes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Func_Tarefa> Func_Tarefa { get; set; }
        public virtual Manifestos Manifestos { get; set; }
    }
}