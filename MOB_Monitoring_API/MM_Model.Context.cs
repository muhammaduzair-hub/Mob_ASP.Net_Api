﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MOB_Monitoring_API
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MOB_MonitoringEntities : DbContext
    {
        public MOB_MonitoringEntities()
            : base("name=MOB_MonitoringEntities")
        {
            Configuration.ProxyCreationEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Ariel_shots> Ariel_shots { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<employee> employees { get; set; }
        public virtual DbSet<MOB> MOBs { get; set; }
        public virtual DbSet<zone> zones { get; set; }
        public virtual DbSet<zone_detail> zone_detail { get; set; }
    }
}