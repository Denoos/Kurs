using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace APIKurs.Models;

public partial class QwertyContext : DbContext
{
    public QwertyContext()
    {
    }

    public QwertyContext(DbContextOptions<QwertyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Condition> Conditions { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Ppe> Ppes { get; set; }

    public virtual DbSet<PpeType> PpeTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=127.0.0.1;user=root;password=;database=_qwerty", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.39-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Condition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("condition");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("title");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("people");

            entity.HasIndex(e => e.PostId, "FK_people");

            entity.HasIndex(e => e.StatusId, "FK_people_status_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .HasColumnName("name");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(255)
                .HasColumnName("patronymic");
            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.StatusId)
                .HasColumnType("int(11)")
                .HasColumnName("status_id");
            entity.Property(e => e.Surname)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("surname");

            entity.HasOne(d => d.Post).WithMany(p => p.People)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_people");

            entity.HasOne(d => d.Status).WithMany(p => p.People)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_people_status_id");

            entity.HasMany(d => d.IdPpes).WithMany(p => p.IdPeople)
                .UsingEntity<Dictionary<string, object>>(
                    "CrossPpePerson",
                    r => r.HasOne<Ppe>().WithMany()
                        .HasForeignKey("IdPpe")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_cross_ppe_people_ppe_id"),
                    l => l.HasOne<Person>().WithMany()
                        .HasForeignKey("IdPeople")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_cross_ppe_people_people_id"),
                    j =>
                    {
                        j.HasKey("IdPeople", "IdPpe")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("cross_ppe_people");
                        j.HasIndex(new[] { "IdPpe" }, "FK_cross_ppe_people_ppe_id");
                        j.IndexerProperty<int>("IdPeople")
                            .HasColumnType("int(11)")
                            .HasColumnName("id_people");
                        j.IndexerProperty<int>("IdPpe")
                            .HasColumnType("int(11)")
                            .HasColumnName("id_ppe");
                    });
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("post");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("title");
        });

        modelBuilder.Entity<Ppe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ppe");

            entity.HasIndex(e => e.ConditionId, "FK_ppe_condition_id");

            entity.HasIndex(e => e.TypeId, "FK_ppe_ppe_type_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ConditionId)
                .HasColumnType("int(11)")
                .HasColumnName("condition_id");
            entity.Property(e => e.DateEnd)
                .HasDefaultValueSql("'0000-01-01'")
                .HasColumnName("date_end");
            entity.Property(e => e.DateGet)
                .HasDefaultValueSql("'0000-01-01'")
                .HasColumnName("date_get");
            entity.Property(e => e.InventoryNumber)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("inventory_number");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("title");
            entity.Property(e => e.TypeId)
                .HasColumnType("int(11)")
                .HasColumnName("type_id");

            entity.HasOne(d => d.Condition).WithMany(p => p.Ppes)
                .HasForeignKey(d => d.ConditionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ppe_condition_id");

            entity.HasOne(d => d.Type).WithMany(p => p.Ppes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ppe_ppe_type_id");
        });

        modelBuilder.Entity<PpeType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ppe_type");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("title");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Ttle)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("ttle");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("status");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity
                .ToTable("users");

            entity.HasIndex(e => e.IdRole, "FK_users_roles_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IdRole)
                .HasColumnType("int(11)")
                .HasColumnName("id_role");
            entity.Property(e => e.Login)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(511)
                .HasDefaultValueSql("''")
                .HasColumnName("password");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");

            entity.HasOne(d => d.IdRoleNavigation).WithMany()
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_users_roles_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
