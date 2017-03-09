namespace MyAppTools
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using MyAppTools.Domain;

    public partial class MyAppToolsDBContext : DbContext
    {
        public MyAppToolsDBContext()
            : base("name=MyAppToolsDBContext")
        {
        }

        public virtual DbSet<ClientProfile> ClientProfile { get; set; }
        public virtual DbSet<EventLog> EventLog { get; set; }
        public virtual DbSet<ClientPermissions> ClientPermissions { get; set; }
        public virtual DbSet<ApiEndpoints> ApiEndpoints { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<decimal>().Configure(c => c.HasPrecision(18, 2));

          }
    }
}
