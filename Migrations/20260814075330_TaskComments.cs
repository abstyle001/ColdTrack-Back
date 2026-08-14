using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ColdTrack_Back.Migrations
{
    /// <inheritdoc />
    public partial class TaskComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskComments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskComments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TaskComments_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3119));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3125));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3126));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3127));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3128));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3129));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3130));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3131));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3131));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3133));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3134));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3134));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3135));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3136));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3137));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3137));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3138));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3140));

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Group", "Key", "Name" },
                values: new object[] { 19L, new DateTime(2026, 8, 14, 7, 53, 27, 746, DateTimeKind.Utc).AddTicks(3140), null, "任务管理", "task.comment", "任务评论" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 19L, "7d06dcab-5a11-482e-b015-cf5f6569d5a3" },
                    { 19L, "f0874ebd-be74-4a5e-951a-c738f27f6cb8" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_AuthorId",
                table: "TaskComments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_TaskId",
                table: "TaskComments",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskComments");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 19L, "7d06dcab-5a11-482e-b015-cf5f6569d5a3" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 19L, "f0874ebd-be74-4a5e-951a-c738f27f6cb8" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7093));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7101));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7103));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7104));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7106));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7109));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7110));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7112));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7113));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7115));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7116));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7117));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7118));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7120));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7122));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7124));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7126));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 3, 16, 2, 14, 995, DateTimeKind.Utc).AddTicks(7128));
        }
    }
}
