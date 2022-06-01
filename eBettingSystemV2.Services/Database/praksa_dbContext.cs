using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace eBettingSystemV2.Services.Database
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

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Sport> Sport { get; set; }
        public virtual DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=192.168.43.21;Database=praksa_db;Username=praksa;Password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.Property(e => e.name)
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

                entity.Property(e => e.Teamname).HasColumnName("teamname");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.Countryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("teams_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
