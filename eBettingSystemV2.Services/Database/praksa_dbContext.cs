using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace eBettingSystemV2.Services.DataBase
{
    public partial class praksa_dbContext : DbContext
    {
        public praksa_dbContext()
        {
        }

        public praksa_dbContext(DbContextOptions<praksa_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Competition> Competitions { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(ConfigurationManager.AppSettings["DefaultConnection"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Competition>(entity =>
            {
                entity.ToTable("competition", "BettingSystem");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Countryid).HasColumnName("countryid");

                entity.Property(e => e.Naziv)
                    .HasColumnType("character varying")
                    .HasColumnName("naziv");

                entity.Property(e => e.Sportid).HasColumnName("sportid");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Competitions)
                    .HasForeignKey(d => d.Countryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("competition_fk");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.Competitions)
                    .HasForeignKey(d => d.Sportid)
                    .HasConstraintName("competition_fk_1");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country", "BettingSystem");

                entity.HasIndex(e => e.CountryName, "country_un")
                    .IsUnique();

                entity.Property(e => e.CountryId).HasDefaultValueSql("nextval('\"BettingSystem\".country_countryid_seq'::regclass)");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Sport>(entity =>
            {
                entity.HasKey(e => e.SportsId)
                    .HasName("sportovi_pk");

                entity.ToTable("sport", "BettingSystem");

                entity.Property(e => e.SportsId).HasDefaultValueSql("nextval('\"BettingSystem\".sportovi_id_seq'::regclass)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("teams", "BettingSystem");

                entity.Property(e => e.Teamid).HasColumnName("teamid");

                entity.Property(e => e.City)
                    .HasColumnType("character varying")
                    .HasColumnName("city");

                entity.Property(e => e.Countryid).HasColumnName("countryid");

                entity.Property(e => e.Foundedyear).HasColumnName("foundedyear");

                entity.Property(e => e.Sportid).HasColumnName("sportid");

                entity.Property(e => e.Teamname).HasColumnName("teamname");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.Countryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("teams_fk");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.Sportid)
                    .HasConstraintName("teams_sportid_sport_sportsid_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
