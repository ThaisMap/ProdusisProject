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
    
    public partial class Funcionarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Funcionarios()
        {
            this.Func_Tarefa = new HashSet<Func_Tarefa>();
            this.Observacoes = new HashSet<Observacoes>();
        }
    
        public int idFunc { get; set; }
        public string nomeFunc { get; set; }
        public string matriculaFunc { get; set; }
        public string tipoFunc { get; set; }
        public string senhaFunc { get; set; }
        public bool ocupadoFunc { get; set; }
        public bool ativoFunc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Func_Tarefa> Func_Tarefa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Observacoes> Observacoes { get; set; }
    }
}
