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
    
    public partial class Observacoes
    {
        public int idObs { get; set; }
        public int FuncObs { get; set; }
        public System.DateTime DataObs { get; set; }
        public string TextoObs { get; set; }
    
        public virtual Funcionarios Funcionarios { get; set; }
    }
}