using Microsoft.EntityFrameworkCore;
using Rent.DAL.Models;
using temp;

namespace Rent.DAL.Context;

public sealed partial class RentContext : DbContext
{
    public RentContext()
    {
    }

    public RentContext(DbContextOptions<RentContext> options)
        : base(options)
    {
    }

    public required DbSet<Accommodation> Accommodations { get; set; }

    public required DbSet<AccommodationRoom> AccommodationRooms { get; set; }

    public required DbSet<Address> Addresses { get; set; }

    public required DbSet<Asset> Assets { get; set; }

    public required DbSet<Bill> Bills { get; set; }

    public required DbSet<Impost> Imposts { get; set; }

    public required DbSet<Owner> Owners { get; set; }

    public required DbSet<Payment> Payments { get; set; }

    public required DbSet<Price> Prices { get; set; }

    public required DbSet<Models.Rent> Rents { get; set; }

    public required DbSet<Room> Rooms { get; set; }

    public required DbSet<RoomType> RoomTypes { get; set; }

    public required DbSet<Tenant> Tenants { get; set; }

    public required DbSet<VwCertificateForTenant> VwCertificateForTenants { get; set; }

    public required DbSet<VwRoomsWithTenant> VwRoomsWithTenants { get; set; }

    public required DbSet<VwTenantAssetPayment> VwTenantAssetPayments { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accommodation>(entity =>
        {
            entity.ToTable("Accommodation");

            entity.Property(e => e.AccommodationId).ValueGeneratedNever();
        });

        modelBuilder.Entity<AccommodationRoom>(entity =>
        {
            entity.ToTable("AccommodationRoom");

            entity.Property(e => e.AccommodationRoomId).ValueGeneratedNever();

            entity.HasOne(d => d.Accommodation).WithMany(p => p.AccommodationRooms)
                .HasForeignKey(d => d.AccommodationId);

            entity.HasOne(d => d.Room).WithMany(p => p.AccommodationRooms)
                .HasForeignKey(d => d.RoomId);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");

            entity.Property(e => e.AddressId).ValueGeneratedNever();

            entity.HasMany(e => e.Tenants)
                .WithOne(e => e.Address)
                .HasForeignKey(e => e.AddressId);
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.ToTable("Asset");

            entity.Property(e => e.AssetId).ValueGeneratedNever();

            entity.HasOne(d => d.Owner).WithMany(p => p.Assets)
                .HasForeignKey(d => d.OwnerId);

            entity.HasOne(d => d.Room).WithMany(p => p.Assets)
                .HasForeignKey(d => d.RoomId);
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.ToTable("Bill");

            entity.Property(e => e.BillId).ValueGeneratedNever();

            entity.HasOne(d => d.Rent).WithMany(p => p.Bills)
                .HasForeignKey(d => d.RentId);

            entity.HasOne(d => d.Tenant).WithMany(p => p.Bills)
                .HasForeignKey(d => d.TenantId);
        });

        modelBuilder.Entity<Impost>(entity =>
        {
            entity.ToTable("Impost");

            entity.Property(e => e.ImpostId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.ToTable("Owner");

            entity.Property(e => e.OwnerId).ValueGeneratedNever();

            entity.HasOne(d => d.Address).WithMany(p => p.Owners)
                .HasForeignKey(d => d.AddressId);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.Bill).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BillId);

            entity.HasOne(d => d.Tenant).WithMany(p => p.Payments)
                .HasForeignKey(d => d.TenantId);
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity
                .ToTable("Price");

            entity.Property(e => e.PriceId).ValueGeneratedNever();

            entity.HasOne(d => d.RoomType).WithMany(p => p.Prices)
                .HasForeignKey(d => d.RoomTypeId);
        });

        modelBuilder.Entity<Models.Rent>(entity =>
        {
            entity.ToTable("Rent");

            entity.Property(e => e.RentId).ValueGeneratedNever();

            entity.HasOne(d => d.Asset).WithMany(p => p.Rents)
                .HasForeignKey(d => d.AssetId);

            entity.HasOne(d => d.Tenant).WithMany(p => p.Rents)
                .HasForeignKey(d => d.TenantId);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("Room");

            entity.Property(e => e.RoomId).ValueGeneratedNever();

            entity.HasOne(d => d.Address).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.AddressId);

            entity.HasOne(d => d.RoomType).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomTypeId);
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.ToTable("RoomType");

            entity.Property(e => e.RoomTypeId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.ToTable("Tenant");

            entity.Property(e => e.TenantId).ValueGeneratedNever();
        });

        modelBuilder.Entity<VwCertificateForTenant>(entity =>
        {
            entity.HasNoKey().ToView("vw_Certificate_For_Tenant");
        });

        modelBuilder.Entity<VwRoomsWithTenant>(entity =>
        {
            entity.HasNoKey().ToView("vw_Rooms_With_Tenants");
        });

        modelBuilder.Entity<VwTenantAssetPayment>(entity =>
        {
            entity.HasNoKey().ToView("vw_Tenant_Asset_Payment");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
