using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ColdTrack_Back.Migrations
{
    /// <inheritdoc />
    public partial class TagSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskTags",
                columns: table => new
                {
                    TaskId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTags", x => new { x.TaskId, x.TagId });
                    table.ForeignKey(
                        name: "FK_TaskTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTags_Tasks_TaskId",
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
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7178));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7182));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7184));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7185));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7186));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7187));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7188));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7189));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7190));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7192));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7192));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7193));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7194));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7195));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7196));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7197));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7197));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7199));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 19L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7200));

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Group", "Key", "Name" },
                values: new object[,]
                {
                    { 20L, new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7200), null, "任务管理", "tag.read", "标签查看" },
                    { 21L, new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7201), null, "任务管理", "tag.create", "标签创建" },
                    { 22L, new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7202), null, "任务管理", "tag.update", "标签编辑" },
                    { 23L, new DateTime(2026, 8, 21, 9, 42, 11, 476, DateTimeKind.Utc).AddTicks(7203), null, "任务管理", "tag.delete", "标签删除" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 20L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 21L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 22L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 23L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 20L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskTags_TagId",
                table: "TaskTags",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskTags");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 20L, "417355cb-7f8b-4628-b6c9-c34af297ea67" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 21L, "417355cb-7f8b-4628-b6c9-c34af297ea67" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 22L, "417355cb-7f8b-4628-b6c9-c34af297ea67" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 23L, "417355cb-7f8b-4628-b6c9-c34af297ea67" });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 20L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6258));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6261));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6263));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6264));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6265));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6267));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6267));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6268));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6269));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6270));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6271));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6272));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6273));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6274));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6275));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6275));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6276));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6277));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 19L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 8, 9, 54, 599, DateTimeKind.Utc).AddTicks(6278));
        }
    }
}
