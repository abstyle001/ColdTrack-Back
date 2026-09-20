using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ColdTrack_Back.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class PostgresInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    NickName = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<string>(type: "text", nullable: false),
                    MaxSeq = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Explain = table.Column<string>(type: "text", nullable: false),
                    ManagerId = table.Column<string>(type: "text", nullable: false),
                    Workspace = table.Column<string>(type: "text", nullable: false),
                    Addition = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscardDepartments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<string>(type: "text", nullable: false),
                    ChildId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscardDepartments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Group = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PositionDepartments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PositionId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionDepartments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Duty = table.Column<string>(type: "text", nullable: false),
                    Workspace = table.Column<string>(type: "text", nullable: false),
                    Addition = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    PermissionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPositions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    PositionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    AssigneeId = table.Column<string>(type: "text", nullable: true),
                    CreatorId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Deadline = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskComments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorId = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "417355cb-7f8b-4628-b6c9-c34af297ea67", null, "Admin", "ADMIN" },
                    { "a96a582b-2ab9-4528-8d45-b3a78f552e0f", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Avatar", "City", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NickName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "de4d5418-88ef-4d01-80a4-dfc971ae3d47", 0, null, "北京市", "1dbe8e32-024a-4d48-9423-000b22811b2f", new DateTime(2026, 4, 28, 16, 14, 18, 972, DateTimeKind.Utc).AddTicks(5242), "admin@cold.com", false, true, null, "Admin", "ADMIN@COLD.COM", "ADMIN@COLD.COM", "AQAAAAIAAYagAAAAELUl/KphdKnCALf14U8IuxYFjhP9bK0HguKM2DlGh5bT/RGf17rpOiH4f7FnpzidgA==", "17323895436", false, "c25b3ec0-5b13-4e4a-8238-05a87542ada6", false, "admin@cold.com" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Group", "Key", "Name" },
                values: new object[,]
                {
                    { 1L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "用户管理", "user.read", "用户查看" },
                    { 2L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "用户管理", "user.create", "用户创建" },
                    { 3L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "用户管理", "user.update", "用户编辑" },
                    { 4L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "用户管理", "user.delete", "用户删除" },
                    { 5L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "用户管理", "user.assign", "用户分配职位" },
                    { 6L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "部门管理", "department.read", "部门查看" },
                    { 7L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "部门管理", "department.create", "部门创建" },
                    { 8L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "部门管理", "department.update", "部门编辑" },
                    { 9L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "部门管理", "department.delete", "部门删除" },
                    { 10L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "职位管理", "position.read", "职位查看" },
                    { 11L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "职位管理", "position.create", "职位创建" },
                    { 12L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "职位管理", "position.update", "职位编辑" },
                    { 13L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "职位管理", "position.delete", "职位删除" },
                    { 14L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "任务管理", "task.read", "任务查看" },
                    { 15L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "任务管理", "task.create", "任务创建" },
                    { 16L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "任务管理", "task.update", "任务编辑" },
                    { 17L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "任务管理", "task.delete", "任务删除" },
                    { 18L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "系统设置", "role.manage", "角色与权限管理" },
                    { 19L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "任务管理", "task.comment", "任务评论" },
                    { 20L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "任务管理", "tag.read", "标签查看" },
                    { 21L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "任务管理", "tag.create", "标签创建" },
                    { 22L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "任务管理", "tag.update", "标签编辑" },
                    { 23L, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "任务管理", "tag.delete", "标签删除" }
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
                    { 19L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 20L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 21L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 22L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 23L, "417355cb-7f8b-4628-b6c9-c34af297ea67" },
                    { 1L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" },
                    { 3L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" },
                    { 6L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" },
                    { 10L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" },
                    { 14L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" },
                    { 19L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" },
                    { 20L, "a96a582b-2ab9-4528-8d45-b3a78f552e0f" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "417355cb-7f8b-4628-b6c9-c34af297ea67", "de4d5418-88ef-4d01-80a4-dfc971ae3d47" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_AuthorId",
                table: "TaskComments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_TaskId",
                table: "TaskComments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssigneeId",
                table: "Tasks",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatorId",
                table: "Tasks",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Status",
                table: "Tasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTags_TagId",
                table: "TaskTags",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "DiscardDepartments");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "PositionDepartments");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "TaskComments");

            migrationBuilder.DropTable(
                name: "TaskTags");

            migrationBuilder.DropTable(
                name: "UserPositions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
