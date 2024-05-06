using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rent.Auth.DAL.Models;
using System.Collections.Generic;

namespace Rent.Auth.DAL.Configurations;

public static class SeedData
{
    public static ModelBuilder Seed(this ModelBuilder builder)
    {
        builder.SeedRoles();
        builder.SeedAdmin();
        builder.SeedUserRoles();

        return builder;
    }

    private static ModelBuilder SeedRoles( this ModelBuilder builder)
    {
        builder.Entity<Role>().HasData(
            new Role() { Id = new Guid("431f29e9-13ff-4f5f-b178-511610d5103f"), Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
            new Role() { Id = new Guid("5adbec33-97c5-4041-be6a-e0f3d3ca6f44"), Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
        );

        return builder;
    }

    private static ModelBuilder SeedAdmin( this ModelBuilder builder)
    {
        var hasher = new PasswordHasher<User>();
        builder.Entity<User>().HasData(
            new User
            {
                Id = new Guid("f6de94e2-9265-49ef-b8fe-a522cad66a99"),
                UserName = "Admin",
                Email = "Admin@gmail.com",
                EmailConfirmed = true,
                NormalizedUserName = "ADMIN",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                PasswordHash = hasher.HashPassword(null, "AdminPassword"),
                SecurityStamp = Guid.NewGuid().ToString(),
            }
        );

        return builder;
    }

    private static ModelBuilder SeedUserRoles( this ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                RoleId = new Guid("431f29e9-13ff-4f5f-b178-511610d5103f"),
                UserId = new Guid("f6de94e2-9265-49ef-b8fe-a522cad66a99")
            }
        );

        return builder;
    }
}