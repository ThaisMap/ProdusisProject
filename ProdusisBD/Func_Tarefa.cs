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
    
    public partial class Func_Tarefa
    {
        public int idFuncTarefa { get; set; }
        public int Funcionario { get; set; }
        public int Tarefa { get; set; }
        public Nullable<float> Pontuacao { get; set; }
    
        public virtual Funcionarios Funcionarios { get; set; }
        public virtual Tarefas Tarefas { get; set; }
    }
}
