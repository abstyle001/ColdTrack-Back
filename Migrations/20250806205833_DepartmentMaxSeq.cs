using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ColdTrack_Back.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentMaxSeq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3d3433f-ca1c-49f7-aaab-964c977eab82");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "457192a0-541a-40cf-aae2-aaff4f3ed601", "87573c09-63d8-4cb8-8966-b2bc21b5a5a4" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "457192a0-541a-40cf-aae2-aaff4f3ed601");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "87573c09-63d8-4cb8-8966-b2bc21b5a5a4");

            migrationBuilder.AddColumn<string>(
                name: "MaxSeq",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "MaxSeq",
                table: "Departments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "457192a0-541a-40cf-aae2-aaff4f3ed601", null, "Admin", "ADMIN" },
                    { "a3d3433f-ca1c-49f7-aaab-964c977eab82", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Avatar", "City", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NickName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "87573c09-63d8-4cb8-8966-b2bc21b5a5a4", 0, null, "北京市", "f94d1c78-8881-4026-9fcc-130025b74a4b", new DateTime(2025, 8, 5, 21, 10, 18, 841, DateTimeKind.Utc).AddTicks(8138), "admin@cold.com", false, true, null, "Admin", "ADMIN@COLD.COM", "ADMIN@COLD.COM", "AQAAAAIAAYagAAAAEN74YQF5HOAj5SVttYLhbOeDBK7FFl4o1f87sLmUneGOyII15AN/w6bOGdE09Ti/7A==", "17323895436", false, "05234f67-fc84-4b1c-8e67-8e4675719da5", false, "admin@cold.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "457192a0-541a-40cf-aae2-aaff4f3ed601", "87573c09-63d8-4cb8-8966-b2bc21b5a5a4" });
        }
    }
}
