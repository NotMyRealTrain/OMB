using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OMB.Api.Models;
using OMB.Api.Enums;

namespace OMB.Api.Data;

public partial class OmbDbContext : DbContext
{
    public OmbDbContext(DbContextOptions<OmbDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDayEntry> OrderDayEntries { get; set; }

    public virtual DbSet<OrderResidentDayEntry> OrderResidentDayEntries { get; set; }

    public virtual DbSet<Resident> Residents { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Week> Weeks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum<OrderStatus>()
            .HasPostgresEnum<RoleName>()
            .HasPostgresEnum<BirthdayMeal>();

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("locations_pkey");

            entity.ToTable("locations");

            entity.HasIndex(e => e.Code, "locations_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.Code)
                .HasColumnType("character varying")
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.HasIndex(e => e.LocationId, "orders_location_id_idx");

            entity.HasIndex(e => new { e.LocationId, e.WeekId }, "orders_location_id_week_id_idx").IsUnique();

            entity.HasIndex(e => e.WeekId, "orders_week_id_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.GroupLeaderName)
                .HasColumnType("character varying")
                .HasColumnName("group_leader_name");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasColumnType("order_status")
                .HasDefaultValueSql("'DRAFT'::order_status");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.LockedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("locked_at");
            entity.Property(e => e.LockedByUserId).HasColumnName("locked_by_user_id");
            entity.Property(e => e.SubmittedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("submitted_at");
            entity.Property(e => e.SubmittedByUserId).HasColumnName("submitted_by_user_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.WeekId).HasColumnName("week_id");

            entity.HasOne(d => d.Location).WithMany(p => p.Orders)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_location_id_fkey");

            entity.HasOne(d => d.LockedByUser).WithMany(p => p.OrderLockedByUsers)
                .HasForeignKey(d => d.LockedByUserId)
                .HasConstraintName("orders_locked_by_user_id_fkey");

            entity.HasOne(d => d.SubmittedByUser).WithMany(p => p.OrderSubmittedByUsers)
                .HasForeignKey(d => d.SubmittedByUserId)
                .HasConstraintName("orders_submitted_by_user_id_fkey");

            entity.HasOne(d => d.Week).WithMany(p => p.Orders)
                .HasForeignKey(d => d.WeekId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_week_id_fkey");
        });

        modelBuilder.Entity<OrderDayEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_day_entries_pkey");

            entity.ToTable("order_day_entries");

            entity.HasIndex(e => new { e.OrderId, e.ServiceDate }, "order_day_entries_order_id_service_date_idx").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExtraMeals)
                .HasDefaultValue(0)
                .HasColumnName("extra_meals");
            entity.Property(e => e.ExtraVegMeals)
                .HasDefaultValue(0)
                .HasColumnName("extra_veg_meals");
            entity.Property(e => e.NotesDay)
                .HasDefaultValueSql("''::text")
                .HasColumnName("notes_day");
            entity.Property(e => e.BirthdayMeal)
                .HasColumnName("birthday")
                .HasColumnType("birthday_meal")
                .HasDefaultValueSql("'NONE'::birthday_meal");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ServiceDate).HasColumnName("service_date");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDayEntries)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_day_entries_order_id_fkey");
        });

        modelBuilder.Entity<OrderResidentDayEntry>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ResidentId, e.ServiceDate }).HasName("order_resident_day_entries_pkey");

            entity.ToTable("order_resident_day_entries");

            entity.HasIndex(e => e.LocationId, "order_resident_day_entries_location_id_idx");

            entity.HasIndex(e => new { e.OrderId, e.ServiceDate }, "order_resident_day_entries_order_id_service_date_idx");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ResidentId).HasColumnName("resident_id");
            entity.Property(e => e.ServiceDate).HasColumnName("service_date");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.Present)
                .HasDefaultValue(true)
                .HasColumnName("present");

            entity.HasOne(d => d.Location).WithMany(p => p.OrderResidentDayEntries)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_resident_day_entries_location_id_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderResidentDayEntries)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_resident_day_entries_order_id_fkey");

            entity.HasOne(d => d.Resident).WithMany(p => p.OrderResidentDayEntries)
                .HasForeignKey(d => d.ResidentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_resident_day_entries_resident_id_fkey");
        });

        modelBuilder.Entity<Resident>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("residents_pkey");

            entity.ToTable("residents");

            entity.HasIndex(e => new { e.LastName, e.FirstName }, "residents_last_name_first_name_idx");

            entity.HasIndex(e => e.LocationId, "residents_location_id_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AllergenNotes)
                .HasDefaultValueSql("''::text")
                .HasColumnName("allergen_notes");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FirstName)
                .HasColumnType("character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.IddsiLevel).HasColumnName("iddsi_level");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsVegetarian)
                .HasDefaultValue(false)
                .HasColumnName("is_vegetarian");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Location).WithMany(p => p.Residents)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("residents_location_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasColumnType("role_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => new { e.AuthProvider, e.AuthSubject }, "users_auth_provider_auth_subject_idx").IsUnique();

            entity.HasIndex(e => e.DefaultLocationId, "users_default_location_id_idx");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthProvider)
                .HasColumnType("character varying")
                .HasColumnName("auth_provider");
            entity.Property(e => e.AuthSubject)
                .HasColumnType("character varying")
                .HasColumnName("auth_subject");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DefaultLocationId).HasColumnName("default_location_id");
            entity.Property(e => e.DisplayNameFirst)
                .HasColumnType("character varying")
                .HasColumnName("display_name_first");
            entity.Property(e => e.DisplayNameLast)
                .HasColumnType("character varying")
                .HasColumnName("display_name_last");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");

            entity.HasOne(d => d.DefaultLocation).WithMany(p => p.Users)
                .HasForeignKey(d => d.DefaultLocationId)
                .HasConstraintName("users_default_location_id_fkey");

            entity.HasMany(d => d.Locations).WithMany(p => p.UsersNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "UserLocation",
                    r => r.HasOne<Location>().WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("user_locations_location_id_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("user_locations_user_id_fkey"),
                    j =>
                    {
                        j.HasKey("UserId", "LocationId").HasName("user_locations_pkey");
                        j.ToTable("user_locations");
                        j.IndexerProperty<long>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<long>("LocationId").HasColumnName("location_id");
                    });
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("user_roles_pkey");

            entity.ToTable("user_roles");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("assigned_at");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_roles_role_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_roles_user_id_fkey");
        });

        modelBuilder.Entity<Week>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("weeks_pkey");

            entity.ToTable("weeks");

            entity.HasIndex(e => new { e.IsoYear, e.IsoWeek }, "weeks_iso_year_iso_week_idx").IsUnique();

            entity.HasIndex(e => e.WeekStartDate, "weeks_week_start_date_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsoWeek).HasColumnName("iso_week");
            entity.Property(e => e.IsoYear).HasColumnName("iso_year");
            entity.Property(e => e.WeekStartDate).HasColumnName("week_start_date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
