using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ColdTrack_Back.Migrations
{
    /// <inheritdoc />
    public partial class RbacPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ddd4d91-8aaa-40a5-bf91-684d576ec7d1");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "aa3a717f-a0b1-43d3-8758-2ec117c37b19", "fb8a3d98-4d26-483c-8237-135d6884b93b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aa3a717f-a0b1-43d3-8758-2ec117c37b19");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fb8a3d98-4d26-483c-8237-135d6884b93b");

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Group", "Key", "Name" },
                values: new object[,]
                {
                    { 1L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5395), null, "用户管理", "user.read", "用户查看" },
                    { 2L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5409), null, "用户管理", "user.create", "用户创建" },
                    { 3L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5412), null, "用户管理", "user.update", "用户编辑" },
                    { 4L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5414), null, "用户管理", "user.delete", "用户删除" },
                    { 5L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5416), null, "用户管理", "user.assign", "用户分配职位" },
                    { 6L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5421), null, "部门管理", "department.read", "部门查看" },
                    { 7L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5423), null, "部门管理", "department.create", "部门创建" },
                    { 8L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5425), null, "部门管理", "department.update", "部门编辑" },
                    { 9L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5427), null, "部门管理", "department.delete", "部门删除" },
                    { 10L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5430), null, "职位管理", "position.read", "职位查看" },
                    { 11L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5432), null, "职位管理", "position.create", "职位创建" },
                    { 12L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5434), null, "职位管理", "position.update", "职位编辑" },
                    { 13L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5436), null, "职位管理", "position.delete", "职位删除" },
                    { 14L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5438), null, "任务管理", "task.read", "任务查看" },
                    { 15L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5439), null, "任务管理", "task.create", "任务创建" },
                    { 16L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5441), null, "任务管理", "task.update", "任务编辑" },
                    { 17L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5443), null, "任务管理", "task.delete", "任务删除" },
                    { 18L, new DateTime(2026, 7, 11, 15, 6, 34, 128, DateTimeKind.Utc).AddTicks(5447), null, "系统设置", "role.manage", "角色与权限管理" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 1L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 2L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 3L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 4L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 5L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 6L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 7L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 8L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 9L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 10L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 11L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 12L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 13L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 14L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 15L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 16L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 17L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 18L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 1L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" },
                    { 3L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" },
                    { 6L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" },
                    { 10L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" },
                    { 14L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5ddd4d91-8aaa-40a5-bf91-684d576ec7d1", null, "User", "USER" },
                    { "aa3a717f-a0b1-43d3-8758-2ec117c37b19", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Avatar", "City", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NickName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "fb8a3d98-4d26-483c-8237-135d6884b93b", 0, null, "北京市", "87ab8fcd-1bd0-4ffd-afe0-c5affd7c6a07", new DateTime(2026, 7, 9, 15, 34, 52, 455, DateTimeKind.Utc).AddTicks(5161), "admin@cold.com", false, true, null, "Admin", "ADMIN@COLD.COM", "ADMIN@COLD.COM", "AQAAAAIAAYagAAAAEKQfgU1XYVReb+lFeybC0td6DLjSH3kRhB6hY6FBgJsasGKycHVWLDPRM1KoeIR2zQ==", "17323895436", false, "5b59ed15-7706-446d-b078-52e109e18745", false, "admin@cold.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "aa3a717f-a0b1-43d3-8758-2ec117c37b19", "fb8a3d98-4d26-483c-8237-135d6884b93b" });
        }
    }
}
