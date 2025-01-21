using System;
using System.Collections.Generic;
using ApproveMe.Repository.Entities;
using Flozacode.Models;
using Microsoft.EntityFrameworkCore;

namespace ApproveMe.Repository.Contexts;

public partial class AppDbContext : Dbs
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValue(1L)
                .HasColumnName("created_by");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasDefaultValue(1L)
                .HasColumnName("modified_by");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.Username, "user_username_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValue(1L)
                .HasColumnName("created_by");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasDefaultValue(1L)
                .HasColumnName("modified_by");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_session_pkey");

            entity.ToTable("user_session");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsValid)
                .HasDefaultValue(true)
                .HasColumnName("is_valid");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserSessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_session_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
