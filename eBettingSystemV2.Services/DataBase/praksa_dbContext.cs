﻿using System;
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

        public virtual DbSet<competition> Competitions { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<events> Events { get; set; }
        public virtual DbSet<Logcompetition> Logcompetitions { get; set; }
        public virtual DbSet<LogcompetitionTest> LogcompetitionTests { get; set; }
        public virtual DbSet<sport> Sports { get; set; }
        public virtual DbSet<teams> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql(ConfigurationManager.AppSettings["DefaultConnection"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<competition>(entity =>
            {
                entity.ToTable("competition", "BettingSystem");

                entity.Property(e => e.id).HasColumnName("id");

                entity.Property(e => e.countryid).HasColumnName("countryid");

                entity.Property(e => e.naziv)
                    .HasColumnType("character varying")
                    .HasColumnName("naziv");

                entity.Property(e => e.sportid).HasColumnName("sportid");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Competitions)
                    .HasForeignKey(d => d.countryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("competition_fk");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.Competitions)
                    .HasForeignKey(d => d.sportid)
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

            modelBuilder.Entity<events>(entity =>
            {

                entity.ToTable("events", "BettingSystem");

                entity.HasKey(e => e.event_id)
                 .HasName("event_id");

                entity.Property(e => e.event_id)
                    .HasColumnName("event_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.away_team)
                    .HasColumnType("character varying")
                    .HasColumnName("away_team");

                entity.Property(e => e.eventkey)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("eventkey");


                entity.Property(e => e.competition_id).HasColumnName("competition_id");

                entity.Property(e => e.event_name)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("event_name");

                entity.Property(e => e.event_period)
                    .HasColumnType("character varying")
                    .HasColumnName("event_period");

                entity.Property(e => e.event_start_time)
                    .HasColumnType("date")
                    .HasColumnName("event_start_time");

                entity.Property(e => e.event_status)
                    .HasColumnType("character varying")
                    .HasColumnName("event_status");

                entity.Property(e => e.home_team)
                    .HasColumnType("character varying")
                    .HasColumnName("home_team");

                entity.Property(e => e.red_cards_away_team).HasColumnName("red_cards_away_team");

                entity.Property(e => e.red_cards_home_team).HasColumnName("red_cards_home_team");

                entity.Property(e => e.result)
                    .HasColumnType("character varying")
                    .HasColumnName("result");

                entity.Property(e => e.yellow_cards_away_team).HasColumnName("yellow_cards_away_team");

                entity.Property(e => e.yellow_cards_home_team).HasColumnName("yellow_cards_home_team");

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.competition_id)
                    .HasConstraintName("events_fk");
            });

            modelBuilder.Entity<Logcompetition>(entity =>
            {
                entity.ToTable("logcompetition", "BettingSystem");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Naziv)
                    .HasColumnType("character varying")
                    .HasColumnName("naziv");

                entity.Property(e => e.Updated).HasColumnName("updated");
            });

            modelBuilder.Entity<LogcompetitionTest>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("logcompetitionTest", "BettingSystem");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");
            });

            modelBuilder.Entity<sport>(entity =>
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

            modelBuilder.Entity<teams>(entity =>
            {
                entity.HasKey(e => e.teamid)
                    .HasName("teams_pk");

                entity.ToTable("teams", "BettingSystem");

                entity.Property(e => e.teamid).HasColumnName("teamid");

                entity.Property(e => e.city)
                    .HasColumnType("character varying")
                    .HasColumnName("city");

                entity.Property(e => e.countryid).HasColumnName("countryid");

                entity.Property(e => e.foundedyear).HasColumnName("foundedyear");

                entity.Property(e => e.sportid).HasColumnName("sportid");

                entity.Property(e => e.teamname).HasColumnName("teamname");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.countryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("teams_fk");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.sportid)
                    .HasConstraintName("teams_sportid_sport_sportsid_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
