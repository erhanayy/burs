﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAdmin.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DbMyadmEntities : DbContext
    {
        public DbMyadmEntities()
            : base("name=DbMyadmEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tb_city> tb_city { get; set; }
        public virtual DbSet<tb_contact> tb_contact { get; set; }
        public virtual DbSet<tb_county> tb_county { get; set; }
        public virtual DbSet<tb_producer> tb_producer { get; set; }
        public virtual DbSet<tb_user> tb_user { get; set; }
    }
}