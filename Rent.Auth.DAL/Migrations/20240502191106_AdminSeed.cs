using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rent.Auth.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AdminSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiration", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("f6de94e2-9265-49ef-b8fe-a522cad66a99"), 0, "1142233c-dc12-4def-9440-922654e96b4b", "Admin@gmail.com", true, false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEF6sKZ2OYEbW4UZvW48M54KFbL+j5H9IiWLTYbccJKyEdmi2m9T6g8Yur76S0N7exw==", null, false, null, null, "717da2cb-b780-4656-a280-0d587650ad24", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("431f29e9-13ff-4f5f-b178-511610d5103f"), new Guid("f6de94e2-9265-49ef-b8fe-a522cad66a99") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("431f29e9-13ff-4f5f-b178-511610d5103f"), new Guid("f6de94e2-9265-49ef-b8fe-a522cad66a99") });

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("f6de94e2-9265-49ef-b8fe-a522cad66a99"));
        }
    }
}
