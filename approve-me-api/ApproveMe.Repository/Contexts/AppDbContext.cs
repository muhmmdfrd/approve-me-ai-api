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

    public virtual DbSet<Letter> Letters { get; set; }

    public virtual DbSet<LetterApproval> LetterApprovals { get; set; }

    public virtual DbSet<LetterApprovalHistory> LetterApprovalHistories { get; set; }

    public virtual DbSet<LetterAttachment> LetterAttachments { get; set; }

    public virtual DbSet<LetterType> LetterTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<UploadFile> UploadFiles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    public virtual DbSet<Workflow> Workflows { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=11279;Database=approve_me;Username=usr_approve_me;Password=PreeA9DWrhXYyYySpJadUkEwU9;timeout=180");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Letter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("letter_pkey");

            entity.ToTable("letter");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValue(1L)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.LetterDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("letter_date");
            entity.Property(e => e.LetterTypeId).HasColumnName("letter_type_id");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasDefaultValue(1L)
                .HasColumnName("modified_by");
            entity.Property(e => e.StatusId)
                .HasDefaultValue(1)
                .HasColumnName("status_id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Creator).WithMany(p => p.Letters)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("letter_creator_id_fkey");

            entity.HasOne(d => d.LetterType).WithMany(p => p.Letters)
                .HasForeignKey(d => d.LetterTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("letter_letter_type_id_fkey");
        });

        modelBuilder.Entity<LetterApproval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("letter_approval_pkey");

            entity.ToTable("letter_approval");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApprovedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("approved_date");
            entity.Property(e => e.ApproverUserId).HasColumnName("approver_user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValue(1L)
                .HasColumnName("created_by");
            entity.Property(e => e.LetterId).HasColumnName("letter_id");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasDefaultValue(1L)
                .HasColumnName("modified_by");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Token)
                .HasMaxLength(150)
                .HasColumnName("token");

            entity.HasOne(d => d.ApproverUser).WithMany(p => p.LetterApprovals)
                .HasForeignKey(d => d.ApproverUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("letter_approval_approver_user_id_fkey");

            entity.HasOne(d => d.Letter).WithMany(p => p.LetterApprovals)
                .HasForeignKey(d => d.LetterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("letter_approval_letter_id_fkey");
        });

        modelBuilder.Entity<LetterApprovalHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("letter_approval_history_pkey");

            entity.ToTable("letter_approval_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApprovedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("approved_date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValue(1L)
                .HasColumnName("created_by");
            entity.Property(e => e.LetterApprovalId).HasColumnName("letter_approval_id");
            entity.Property(e => e.LetterId).HasColumnName("letter_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Token)
                .HasMaxLength(150)
                .HasColumnName("token");

            entity.HasOne(d => d.LetterApproval).WithMany(p => p.LetterApprovalHistories)
                .HasForeignKey(d => d.LetterApprovalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("letter_approval_history_letter_approval_id_fkey");

            entity.HasOne(d => d.Letter).WithMany(p => p.LetterApprovalHistories)
                .HasForeignKey(d => d.LetterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("letter_approval_history_letter_id_fkey");
        });

        modelBuilder.Entity<LetterAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("letter_attachment_pkey");

            entity.ToTable("letter_attachment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValue(1L)
                .HasColumnName("created_by");
            entity.Property(e => e.IsPrimary)
                .HasDefaultValue(false)
                .HasColumnName("is_primary");
            entity.Property(e => e.LetterId).HasColumnName("letter_id");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasDefaultValue(1L)
                .HasColumnName("modified_by");
            entity.Property(e => e.UploadFileId).HasColumnName("upload_file_id");

            entity.HasOne(d => d.Letter).WithMany(p => p.LetterAttachments)
                .HasForeignKey(d => d.LetterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("letter_attachment_letter_id_fkey");

            entity.HasOne(d => d.UploadFile).WithMany(p => p.LetterAttachments)
                .HasForeignKey(d => d.UploadFileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("letter_attachment_upload_file_id_fkey");
        });

        modelBuilder.Entity<LetterType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("letter_type_pkey");

            entity.ToTable("letter_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
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
            entity.Property(e => e.WorkflowId).HasColumnName("workflow_id");

            entity.HasOne(d => d.Workflow).WithMany(p => p.LetterTypes)
                .HasForeignKey(d => d.WorkflowId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("letter_type_workflow_id_fkey");
        });

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

        modelBuilder.Entity<UploadFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("upload_file_pkey");

            entity.ToTable("upload_file");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValue(1L)
                .HasColumnName("created_by");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .HasColumnName("file_name");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasDefaultValue(1L)
                .HasColumnName("modified_by");
            entity.Property(e => e.Path)
                .HasMaxLength(100)
                .HasColumnName("path");
            entity.Property(e => e.Size).HasColumnName("size");
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

        modelBuilder.Entity<Workflow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("workflow_pkey");

            entity.ToTable("workflow");

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
            entity.Property(e => e.Sequence).HasColumnName("sequence");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Workflows)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("workflow_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
