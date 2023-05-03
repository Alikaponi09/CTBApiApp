using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CTBApiApp.Models;

public partial class TestContext : DbContext
{
    public TestContext()
    {
    }

    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }


    public virtual DbSet<Consignment> Consignments { get; set; }

    public virtual DbSet<ConsignmentPlayer> ConsignmentPlayers { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventPlayer> EventPlayers { get; set; }

    public virtual DbSet<Organizer> Organizers { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-2F32TMB;Database=test;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.AdministratorId).HasName("administrator_administratorid_primary");

            entity.ToTable("Administrator");

            entity.Property(e => e.AdministratorId).HasColumnName("AdministratorID");
            entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");

            entity.HasOne(d => d.Organizer).WithMany(p => p.Administrators)
                .HasForeignKey(d => d.OrganizerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("administrator_organizerid_foreign");
        });

        modelBuilder.Entity<Consignment>(entity =>
        {
            entity.HasKey(e => e.ConsignmentId).HasName("consignment_consignmentid_primary");

            entity.ToTable("Consignment");

            entity.Property(e => e.ConsignmentId).HasColumnName("ConsignmentID");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.TourId).HasColumnName("TourID");

            entity.HasOne(d => d.Status).WithMany(p => p.Consignments)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("consignment_statusid_foreign");

            entity.HasOne(d => d.Tour).WithMany(p => p.Consignments)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("consignment_tourid_foreign");
        });

        modelBuilder.Entity<ConsignmentPlayer>(entity =>
        {
            entity.HasKey(e => e.ConsignmentPlayerId).HasName("consignmentplayer_consignmentplayerid_primary");

            entity.ToTable("ConsignmentPlayer");

            entity.Property(e => e.ConsignmentPlayerId).HasColumnName("ConsignmentPlayerID");
            entity.Property(e => e.ConsignmentId).HasColumnName("ConsignmentID");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

            entity.HasOne(d => d.Consignment).WithMany(p => p.ConsignmentPlayers)
                .HasForeignKey(d => d.ConsignmentId)
                .HasConstraintName("consignmentplayer_consignmentid_foreign");

            entity.HasOne(d => d.Player).WithMany(p => p.ConsignmentPlayers)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("consignmentplayer_playerid_foreign");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("event_eventid_primary");

            entity.ToTable("Event");

            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.DataFinish).HasColumnType("date");
            entity.Property(e => e.DataStart).HasColumnType("date");
            entity.Property(e => e.LocationEvent).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");

            entity.HasOne(d => d.Organizer).WithMany(p => p.Events)
                .HasForeignKey(d => d.OrganizerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("event_organizerid_foreign");

            entity.HasOne(d => d.Status).WithMany(p => p.Events)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("event_statusid_foreign");
        });

        modelBuilder.Entity<EventPlayer>(entity =>
        {
            entity.HasKey(e => e.EventPlayerId).HasName("eventplayer_eventplayerid_primary");

            entity.ToTable("EventPlayer");

            entity.Property(e => e.EventPlayerId).HasColumnName("EventPlayerID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

            entity.HasOne(d => d.Event).WithMany(p => p.EventPlayers)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("eventplayer_eventid_foreign");

            entity.HasOne(d => d.Player).WithMany(p => p.EventPlayers)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("eventplayer_playerid_foreign");
        });

        modelBuilder.Entity<Organizer>(entity =>
        {
            entity.HasKey(e => e.OrganizerId).HasName("organizer_organizerid_primary");

            entity.ToTable("Organizer");

            entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(100);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Fideid).HasName("player_fideid_primary");

            entity.ToTable("Player");

            entity.Property(e => e.Fideid)
                .ValueGeneratedNever()
                .HasColumnName("FIDEID");
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.Contry).HasMaxLength(50);
            entity.Property(e => e.Elorating).HasColumnName("ELORating");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("status_status_primary");

            entity.ToTable("Status");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.TourId).HasName("tour_tourid_primary");

            entity.ToTable("Tour");

            entity.Property(e => e.TourId).HasColumnName("TourID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.NameTour).HasMaxLength(50);

            entity.HasOne(d => d.Event).WithMany(p => p.Tours)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("tour_eventid_foreign");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
