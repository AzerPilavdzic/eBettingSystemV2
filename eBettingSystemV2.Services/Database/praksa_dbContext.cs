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
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Logcompetition> Logcompetitions { get; set; }
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

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("events", "BettingSystem");

                entity.Property(e => e.EventId)
                    .HasColumnName("event_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.AwayTeam)
                    .HasColumnType("character varying")
                    .HasColumnName("away_team");

                entity.Property(e => e.CompetitionId).HasColumnName("competition_id");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("event_name");

                entity.Property(e => e.EventPeriod)
                    .HasColumnType("character varying")
                    .HasColumnName("event_period");

                entity.Property(e => e.EventStartTime)
                    .HasColumnType("date")
                    .HasColumnName("event_start_time");

                entity.Property(e => e.EventStatus)
                    .HasColumnType("character varying")
                    .HasColumnName("event_status");

                entity.Property(e => e.HomeTeam)
                    .HasColumnType("character varying")
                    .HasColumnName("home_team");

                entity.Property(e => e.RedCardsAwayTeam).HasColumnName("red_cards_away_team");

                entity.Property(e => e.RedCardsHomeTeam).HasColumnName("red_cards_home_team");

                entity.Property(e => e.Result)
                    .HasColumnType("character varying")
                    .HasColumnName("result");

                entity.Property(e => e.YellowCardsAwayTeam).HasColumnName("yellow_cards_away_team");

                entity.Property(e => e.YellowCardsHomeTeam).HasColumnName("yellow_cards_home_team");

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("events_fk");
            });

            modelBuilder.Entity<Logcompetition>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("logcompetition", "BettingSystem");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("column1");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Naziv)
                    .HasColumnType("character varying")
                    .HasColumnName("naziv");

                entity.Property(e => e.Updated).HasColumnName("updated");
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
