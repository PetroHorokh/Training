using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Rent.DAL.Models;
using temp;

namespace Rent.DAL.Context;

public partial class RentContext : DbContext
{
    public RentContext()
    {
    }

    public RentContext(DbContextOptions<RentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accommodation> Accommodations { get; set; }

    public virtual DbSet<AccommodationRoom> AccommodationRooms { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Impost> Imposts { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<Rent.DAL.Models.Rent> Rents { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<VwCertificateForTenant> VwCertificateForTenants { get; set; }

    public virtual DbSet<VwRoomsWithTenant> VwRoomsWithTenants { get; set; }

    public virtual DbSet<VwTenantAssetPayment> VwTenantAssetPayments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accommodation>(entity =>
        {
            entity.ToTable("Accommodation");

            entity.Property(e => e.AccommodationId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<AccommodationRoom>(entity =>
        {
            entity.ToTable("AccommodationRoom");

            entity.Property(e => e.AccommodationRoomId).ValueGeneratedNever();

            entity.HasOne(d => d.Accommodation).WithMany(p => p.AccommodationRooms)
                .HasForeignKey(d => d.AccommodationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccommodationRoom_AccommodationId_Accommodation_AccommodationId");

            entity.HasOne(d => d.Room).WithMany(p => p.AccommodationRooms)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccommodationRoom_RoomId_Room_RoomId");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");

            entity.Property(e => e.AddressId).ValueGeneratedNever();
            entity.Property(e => e.Building).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Street).HasMaxLength(255);
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.AssetId).HasName("PK_OwnerShip");

            entity.ToTable("Asset");

            entity.Property(e => e.AssetId).ValueGeneratedNever();

            entity.HasOne(d => d.Owner).WithMany(p => p.Assets)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asset_OwnerId_Owner_OwnerId");

            entity.HasOne(d => d.Room).WithMany(p => p.Assets)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asset_RoomId_Room_RoomId");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.ToTable("Bill");

            entity.Property(e => e.BillId).ValueGeneratedNever();
            entity.Property(e => e.BillAmount).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.Rent).WithMany(p => p.Bills)
                .HasForeignKey(d => d.RentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bill_RentId_Rent_RentId");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Bills)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bill_TenantId_Tenant_TenantId");
        });

        modelBuilder.Entity<Impost>(entity =>
        {
            entity
                .ToTable("Impost")
                .ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.UseHistoryTable("Impost_History", "dbo");
                    ttb
                        .HasPeriodStart("ValidFrom")
                        .HasColumnName("ValidFrom");
                    ttb
                        .HasPeriodEnd("ValidTo")
                        .HasColumnName("ValidTo");
                }));

            entity.Property(e => e.ImpostId).ValueGeneratedNever();
            entity.Property(e => e.Fine).HasColumnType("numeric(3, 2)");
            entity.Property(e => e.Tax).HasColumnType("numeric(4, 2)");
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.ToTable("Owner");

            entity.Property(e => e.OwnerId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Address).WithMany(p => p.Owners)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Owner_AddressId_Address_AddressId");

            entity.HasOne(d => d.User).WithMany(p => p.Owners)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Owner_UserId_User_UserId");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.Bill).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_BillId_Bill_BillId");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Payments)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_TenantId_Tenant_TenantId");
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity
                .ToTable("Price")
                .ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.UseHistoryTable("Price_History", "dbo");
                    ttb
                        .HasPeriodStart("ValidFrom")
                        .HasColumnName("ValidFrom");
                    ttb
                        .HasPeriodEnd("ValidTo")
                        .HasColumnName("ValidTo");
                }));

            entity.Property(e => e.PriceId).ValueGeneratedNever();
            entity.Property(e => e.Value).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.RoomType).WithMany(p => p.Prices)
                .HasForeignKey(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Price_RoomTypeId_RoomType_RoomTypeId");
        });

        modelBuilder.Entity<Rent.DAL.Models.Rent>(entity =>
        {
            entity.ToTable("Rent");

            entity.Property(e => e.RentId).ValueGeneratedNever();

            entity.HasOne(d => d.Asset).WithMany(p => p.Rents)
                .HasForeignKey(d => d.AssetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rent_AssetId_Asset_AssetId");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Rents)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rent_TenantId_Tenant_TenantId");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity
                .ToTable("Role")
                .ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.UseHistoryTable("Role_History", "dbo");
                    ttb
                        .HasPeriodStart("ValidFrom")
                        .HasColumnName("ValidFrom");
                    ttb
                        .HasPeriodEnd("ValidTo")
                        .HasColumnName("ValidTo");
                }));

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.NormalizedName).HasMaxLength(255);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("Room");

            entity.Property(e => e.RoomId).ValueGeneratedNever();
            entity.Property(e => e.Area).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.Address).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_AddressId_Address_AddressId");

            entity.HasOne(d => d.RoomType).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_RoomTypeId_RoomType_RoomTypeId");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.ToTable("RoomType");

            entity.Property(e => e.RoomTypeId).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.ToTable("Tenant");

            entity.Property(e => e.TenantId).ValueGeneratedNever();
            entity.Property(e => e.BankName).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Director).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Address).WithMany(p => p.Tenants)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tenant_AddressId_Address_AddressId");

            entity.HasOne(d => d.User).WithMany(p => p.Tenants)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tenant_UserId_User_UserId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity
                .ToTable("User")
                .ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.UseHistoryTable("User_History", "dbo");
                    ttb
                        .HasPeriodStart("ValidFrom")
                        .HasColumnName("ValidFrom");
                    ttb
                        .HasPeriodEnd("ValidTo")
                        .HasColumnName("ValidTo");
                }));

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(255);
            entity.Property(e => e.NormalizedName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity
                .ToTable("UserRole")
                .ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.UseHistoryTable("UserRole_History", "dbo");
                    ttb
                        .HasPeriodStart("ValidFrom")
                        .HasColumnName("ValidFrom");
                    ttb
                        .HasPeriodEnd("ValidTo")
                        .HasColumnName("ValidTo");
                }));

            entity.Property(e => e.UserRoleId).ValueGeneratedNever();

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_RoleId_Role_RoleId");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_UserId_User_UserId");
        });

        modelBuilder.Entity<VwCertificateForTenant>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Certificate_For_Tenant");

            entity.Property(e => e.RentEndDate).HasColumnName("Rent End Date");
            entity.Property(e => e.RentStartDate).HasColumnName("Rent Start Date");
        });

        modelBuilder.Entity<VwRoomsWithTenant>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Rooms_With_Tenants");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<VwTenantAssetPayment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Tenant_Asset_Payment");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("numeric(37, 4)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
