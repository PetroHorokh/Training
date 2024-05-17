using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rent.Auth.DAL.Configurations;
using Rent.Auth.DAL.Models;

namespace Rent.Auth.DAL.Context;

public sealed partial class AuthRentContext : IdentityDbContext<User, Role, Guid>
{
    public AuthRentContext()
    {
    }

    public AuthRentContext(DbContextOptions<AuthRentContext> options)
        : base(options)
    {
    }

    public new required DbSet<User> Users { get; set; } = null!;

    public required DbSet<Image> Images { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User")
                .HasMany(e => e.Images)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_Image_UserId_User_Id");
        });
        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Images");
        });

        modelBuilder.Seed();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}