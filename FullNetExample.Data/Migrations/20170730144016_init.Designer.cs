using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FullNetExample.Data;

namespace FullNetExample.Data.Migrations
{
    [DbContext(typeof(SamuraiContext))]
    [Migration("20170730144016_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FullNetExample.Domain.Battle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.ToTable("Battles");
                });

            modelBuilder.Entity("FullNetExample.Domain.Quote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastModified");

                    b.Property<int>("SamuraiId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("SamuraiId");

                    b.ToTable("Quotes");
                });

            modelBuilder.Entity("FullNetExample.Domain.Samurai", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Samurais");
                });

            modelBuilder.Entity("FullNetExample.Domain.SamuraiBattle", b =>
                {
                    b.Property<int>("SamuraiId");

                    b.Property<int>("BattleId");

                    b.Property<DateTime>("LastModified");

                    b.HasKey("SamuraiId", "BattleId");

                    b.HasIndex("BattleId");

                    b.ToTable("SamuraiBattles");
                });

            modelBuilder.Entity("FullNetExample.Domain.SecretIdentity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("RealName");

                    b.Property<int>("SamuraiId");

                    b.HasKey("Id");

                    b.HasIndex("SamuraiId")
                        .IsUnique();

                    b.ToTable("SecretIdentity");
                });

            modelBuilder.Entity("FullNetExample.Domain.Quote", b =>
                {
                    b.HasOne("FullNetExample.Domain.Samurai", "Samurai")
                        .WithMany("Quotes")
                        .HasForeignKey("SamuraiId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FullNetExample.Domain.SamuraiBattle", b =>
                {
                    b.HasOne("FullNetExample.Domain.Battle", "Battle")
                        .WithMany("SamuraiBattles")
                        .HasForeignKey("BattleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FullNetExample.Domain.Samurai", "Samurai")
                        .WithMany("SamuraiBattles")
                        .HasForeignKey("SamuraiId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FullNetExample.Domain.SecretIdentity", b =>
                {
                    b.HasOne("FullNetExample.Domain.Samurai", "Smaurai")
                        .WithOne("SecretIdentity")
                        .HasForeignKey("FullNetExample.Domain.SecretIdentity", "SamuraiId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
