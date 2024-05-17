using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rent.Auth.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CloudImageStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Image_ImageId_User_Id",
            //    table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Images",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Data",
                table: "Images",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "Images",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Images",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("f6de94e2-9265-49ef-b8fe-a522cad66a99"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6cb2abb7-8532-44ba-bd6b-baf49ae7de1d", "AQAAAAIAAYagAAAAECif7fN/QJDTvspA2LiVKJNZJSZ3b93TFhHCBqTXMMq/fqYb9F+hyiD6GwWPd5z6fg==", "c6ee7529-6b5e-46b8-a9b0-196d30a1cd00" });

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Image_UserId_User_Id",
            //    table: "Images",
            //    column: "UserId",
            //    principalTable: "User",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Image_UserId_User_Id",
            //    table: "Images");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Data",
                table: "Images",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("f6de94e2-9265-49ef-b8fe-a522cad66a99"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "93e52d21-94d8-41af-bc09-376e43985542", "AQAAAAIAAYagAAAAEEj+QnVVFuJEDf1nnfr/AgE76epbxTToLYbajHCZCq4aKeUfn1+FExIWma9nild4xA==", "b5bfd36a-5286-4f48-88a4-9dc15df5d202" });

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Image_ImageId_User_Id",
            //    table: "Images",
            //    column: "UserId",
            //    principalTable: "User",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
