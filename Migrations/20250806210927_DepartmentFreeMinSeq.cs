using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ColdTrack_Back.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentFreeMinSeq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8b16b89-512f-45ea-99e5-c79b0b2828f8");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "faba22a4-32f5-474a-bda7-f63e4c36c4e3", "215189e3-b994-477c-8fe0-37d04761b792" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "faba22a4-32f5-474a-bda7-f63e4c36c4e3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "215189e3-b994-477c-8fe0-37d04761b792");

            migrationBuilder.AlterColumn<int>(
                name: "ChildId",
                table: "DiscardDepartments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7d06dcab-5a11-482e-b015-cf5f6569d5a3", null, "User", "USER" },
                    { "f0874ebd-be74-4a5e-951a-c738f27f6cb8", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Avatar", "City", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NickName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8a016a79-e64b-4fe7-8522-b3fbbf6978dc", 0, null, "北京市", "7a27ae8e-b1ea-4c4c-a1e7-3dcc0f8fa975", new DateTime(2025, 8, 6, 21, 9, 27, 14, DateTimeKind.Utc).AddTicks(1245), "admin@cold.com", false, true, null, "Admin", "ADMIN@COLD.COM", "ADMIN@COLD.COM", "AQAAAAIAAYagAAAAEN9BTE2hnLufpWx/Msi6TbLkJHB20J/BQy+eGbhlf9o79ZTB5TxKtI3OITHYh9YS9g==", "17323895436", false, "8c15bcb9-c3e2-45f8-8b8d-49a2dbb138a5", false, "admin@cold.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f0874ebd-be74-4a5e-951a-c738f27f6cb8", "8a016a79-e64b-4fe7-8522-b3fbbf6978dc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d06dcab-5a11-482e-b015-cf5f6569d5a3");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f0874ebd-be74-4a5e-951a-c738f27f6cb8", "8a016a79-e64b-4fe7-8522-b3fbbf6978dc" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0874ebd-be74-4a5e-951a-c738f27f6cb8");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8a016a79-e64b-4fe7-8522-b3fbbf6978dc");

            migrationBuilder.AlterColumn<string>(
                name: "ChildId",
                table: "DiscardDepartments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a8b16b89-512f-45ea-99e5-c79b0b2828f8", null, "User", "USER" },
                    { "faba22a4-32f5-474a-bda7-f63e4c36c4e3", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Avatar", "City", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NickName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "215189e3-b994-477c-8fe0-37d04761b792", 0, null, "北京市", "8059ffd8-dd19-4993-9123-7a1c4bbae040", new DateTime(2025, 8, 6, 20, 58, 31, 609, DateTimeKind.Utc).AddTicks(1349), "admin@cold.com", false, true, null, "Admin", "ADMIN@COLD.COM", "ADMIN@COLD.COM", "AQAAAAIAAYagAAAAEAyn5tGh73390rtRg6tVmIOqQkBcTc9ueXQQq4swWEsRvtTRW2kS/8HB3X9gpNxy/w==", "17323895436", false, "6e3e5a70-9bdf-42a6-9a4f-76af04d5f9bd", false, "admin@cold.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "faba22a4-32f5-474a-bda7-f63e4c36c4e3", "215189e3-b994-477c-8fe0-37d04761b792" });
        }
    }
}
