using FullNetExample.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullNetExample.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<SamuraiBattle> SamuraiBattles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });
            //modelBuilder.Entity<Samurai>().Property(s => s.SecretIdentity).IsRequired();

            // shadow state property
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.Name).Property<DateTime>("LastModified");
                modelBuilder.Entity(entityType.Name).Ignore("IsDirty");
            }

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                //"Server = (localdb)\\mssqllocaldb; Database = SamuraiData; Trusted_Connection = True; ",
                "Server = (localdb)\\mssqllocaldb; Database = SamuraiWpfData; Trusted_Connection = True; ",
                options => options.MaxBatchSize(1000)
            );
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries()
             .Where(e => e.State == EntityState.Added ||
                         e.State == EntityState.Modified))
            {
                entry.Property("LastModified").CurrentValue = DateTime.Now;
                ((ClientChangeTracker)entry.Entity).IsDirty = false;
            }
            return base.SaveChanges();
        }
    }
}
