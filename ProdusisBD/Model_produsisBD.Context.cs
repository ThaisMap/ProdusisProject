﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class produsisBDEntities : DbContext
    {
        public produsisBDEntities()
            : base("name=produsisBDEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CapacidadeMotoristas> CapacidadeMotoristas { get; set; }
        public virtual DbSet<Cte_Manifesto> Cte_Manifesto { get; set; }
        public virtual DbSet<Ctes> Ctes { get; set; }
        public virtual DbSet<Func_Tarefa> Func_Tarefa { get; set; }
        public virtual DbSet<Funcionarios> Funcionarios { get; set; }
        public virtual DbSet<Manifestos> Manifestos { get; set; }
        public virtual DbSet<NotasFiscais> NotasFiscais { get; set; }
        public virtual DbSet<Observacoes> Observacoes { get; set; }
        public virtual DbSet<Tarefas> Tarefas { get; set; }
        public virtual DbSet<RelatorioConferencias> RelatorioConferencias { get; set; }
        public virtual DbSet<RelatorioNaoConferencia> RelatorioNaoConferencia { get; set; }
    }
}
