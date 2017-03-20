using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using PalletManagement.Core.Domain;

namespace PalletManagement.Core
{

    public partial class appDbContext : DbContext
    {
        public appDbContext()
            : base("name=DefaultConnection")
        {

        }

        public virtual DbSet<PalletStatus> PalletStatus { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Facility> Facility { get; set; }
        public virtual DbSet<Pallet> Pallet { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<decimal>().Configure(c => c.HasPrecision(18, 2));

        }
    }
}