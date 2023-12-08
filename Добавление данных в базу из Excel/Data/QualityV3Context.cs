using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Добавление_данных_в_базу_из_Excel.Models;

namespace Добавление_данных_в_базу_из_Excel.Data
{
    public partial class QualityV3Context : DbContext
    {
        public QualityV3Context()
        {
        }

        public QualityV3Context(DbContextOptions<QualityV3Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Extruder> Extruders { get; set; } = null!;
        public virtual DbSet<Film> Films { get; set; } = null!;
        public virtual DbSet<OrdersQuality> OrdersQualities { get; set; } = null!;
        public virtual DbSet<StandartQualityFilm> StandartQualityFilms { get; set; } = null!;
        public virtual DbSet<StandartQualityTitle> StandartQualityTitles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=QualityV3;Username=postgres;Password=return;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Extruder>(entity =>
            {
                entity.HasIndex(e => e.Name, "IX_Extruders_Name")
                    .IsUnique();
            });

            modelBuilder.Entity<Film>(entity =>
            {
                entity.HasIndex(e => new { e.Mark, e.Thickness, e.Color }, "IX_Films_Mark_Thickness_Color")
                    .IsUnique();
            });

            modelBuilder.Entity<OrdersQuality>(entity =>
            {
                entity.HasKey(e => e.OrderQualityId);

                entity.ToTable("OrdersQuality");

                entity.HasIndex(e => e.ExtruderId, "IX_OrdersQuality_ExtruderId");

                entity.HasIndex(e => e.FilmId, "IX_OrdersQuality_FilmId");

                entity.HasIndex(e => e.InspectorId, "IX_OrdersQuality_InspectorId");

                entity.HasIndex(e => e.StandartQualityTitleId, "IX_OrdersQuality_StandartQualityTitleId");

                entity.Property(e => e.CreationDate).HasDefaultValueSql("'-infinity'::timestamp with time zone");

                entity.Property(e => e.ElongationAtBreakMd).HasColumnName("ElongationAtBreakMD");

                entity.Property(e => e.ElongationAtBreakTd).HasColumnName("ElongationAtBreakTD");

                entity.Property(e => e.TensileStrengthMd).HasColumnName("TensileStrengthMD");

                entity.Property(e => e.TensileStrengthTd).HasColumnName("TensileStrengthTD");

                entity.HasOne(d => d.Extruder)
                    .WithMany(p => p.OrdersQualities)
                    .HasForeignKey(d => d.ExtruderId);

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.OrdersQualities)
                    .HasForeignKey(d => d.FilmId);

                entity.HasOne(d => d.Inspector)
                    .WithMany(p => p.OrdersQualities)
                    .HasForeignKey(d => d.InspectorId);

                entity.HasOne(d => d.StandartQualityTitle)
                    .WithMany(p => p.OrdersQualities)
                    .HasForeignKey(d => d.StandartQualityTitleId);
            });

            modelBuilder.Entity<StandartQualityFilm>(entity =>
            {
                entity.HasIndex(e => new { e.FilmId, e.StandartQualityTitleId }, "IX_StandartQualityFilms_FilmId_StandartQualityTitleId")
                    .IsUnique();

                entity.HasIndex(e => e.StandartQualityTitleId, "IX_StandartQualityFilms_StandartQualityTitleId");

                entity.Property(e => e.ElongationAtBreakMd).HasColumnName("ElongationAtBreakMD");

                entity.Property(e => e.ElongationAtBreakTd).HasColumnName("ElongationAtBreakTD");

                entity.Property(e => e.TensileStrengthMd).HasColumnName("TensileStrengthMD");

                entity.Property(e => e.TensileStrengthTd).HasColumnName("TensileStrengthTD");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.StandartQualityFilms)
                    .HasForeignKey(d => d.FilmId);

                entity.HasOne(d => d.StandartQualityTitle)
                    .WithMany(p => p.StandartQualityFilms)
                    .HasForeignKey(d => d.StandartQualityTitleId)
                    .HasConstraintName("FK_StandartQualityFilms_StandartQualityTitles_StandartQualityT~");
            });

            modelBuilder.Entity<StandartQualityTitle>(entity =>
            {
                entity.HasIndex(e => e.Title, "IX_StandartQualityTitles_Title")
                    .IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "IX_Users_Email")
                    .IsUnique();

                entity.HasIndex(e => e.Login, "IX_Users_Login")
                    .IsUnique();
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.HasMany(d => d.UsersUsers)
                    .WithMany(p => p.RolesRoles)
                    .UsingEntity<Dictionary<string, object>>(
                        "RoleDtoUserDto",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UsersUserId"),
                        r => r.HasOne<UserRole>().WithMany().HasForeignKey("RolesRoleId"),
                        j =>
                        {
                            j.HasKey("RolesRoleId", "UsersUserId");

                            j.ToTable("RoleDtoUserDto");

                            j.HasIndex(new[] { "UsersUserId" }, "IX_RoleDtoUserDto_UsersUserId");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
