using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FullNetExample.ScaffoldFromDatabase
{
    public partial class SamuraiDataContext : DbContext
    {
        public virtual DbSet<Battles> Battles { get; set; }
        public virtual DbSet<Quotes> Quotes { get; set; }
        public virtual DbSet<SamuraiBattle> SamuraiBattle { get; set; }
        public virtual DbSet<Samurais> Samurais { get; set; }
        public virtual DbSet<SecretIdentity> SecretIdentity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database = SamuraiData; Trusted_Connection = True; ");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quotes>(entity =>
            {
                entity.HasIndex(e => e.SamuraiId)
                    .HasName("IX_Quotes_SamuraiId");
            });

            modelBuilder.Entity<SamuraiBattle>(entity =>
            {
                entity.HasKey(e => new { e.SamuraiId, e.BattleId })
                    .HasName("PK_SamuraiBattle");

                entity.HasIndex(e => e.BattleId)
                    .HasName("IX_SamuraiBattle_BattleId");
            });

            modelBuilder.Entity<SecretIdentity>(entity =>
            {
                entity.HasIndex(e => e.SamuraiId)
                    .HasName("IX_SecretIdentity_SamuraiId")
                    .IsUnique();
            });
        }
    }
}