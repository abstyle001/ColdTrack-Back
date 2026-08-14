using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ColdTrack_Back.Migrations
{
    /// <summary>
    /// 修正 task.comment（权限 19）的角色关联。
    /// TaskComments 迁移曾把该权限关联到数据库中不存在的角色 ID
    /// （f0874ebd / 7d06dcab，系代码中错误的硬编码常量），
    /// 本迁移清理这些错误行，并把权限幂等地关联到数据库实际存在的
    /// Admin（417355cb）与 User（a96a582b）角色。
    /// </summary>
    public partial class FixTaskCommentRoleLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. 删除指向不存在角色的错误关联
            migrationBuilder.Sql(
                "DELETE FROM [RolePermissions] WHERE [PermissionId] = 19 AND [RoleId] IN " +
                "('f0874ebd-be74-4a5e-951a-c738f27f6cb8', '7d06dcab-5a11-482e-b015-cf5f6569d5a3');");

            // 2. 幂等地补充 Admin / User 角色的评论权限
            migrationBuilder.Sql(
                "IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [RoleId] = '417355cb-7f8b-4628-b6c9-c34af297ea67' AND [PermissionId] = 19) " +
                "INSERT INTO [RolePermissions] ([RoleId], [PermissionId]) VALUES ('417355cb-7f8b-4628-b6c9-c34af297ea67', 19);");

            migrationBuilder.Sql(
                "IF NOT EXISTS (SELECT 1 FROM [RolePermissions] WHERE [RoleId] = 'a96a582b-2ab9-4528-8d45-b3a78f552e0f' AND [PermissionId] = 19) " +
                "INSERT INTO [RolePermissions] ([RoleId], [PermissionId]) VALUES ('a96a582b-2ab9-4528-8d45-b3a78f552e0f', 19);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DELETE FROM [RolePermissions] WHERE [PermissionId] = 19 AND [RoleId] IN " +
                "('417355cb-7f8b-4628-b6c9-c34af297ea67', 'a96a582b-2ab9-4528-8d45-b3a78f552e0f');");

            migrationBuilder.Sql(
                "INSERT INTO [RolePermissions] ([RoleId], [PermissionId]) VALUES ('f0874ebd-be74-4a5e-951a-c738f27f6cb8', 19);");

            migrationBuilder.Sql(
                "INSERT INTO [RolePermissions] ([RoleId], [PermissionId]) VALUES ('7d06dcab-5a11-482e-b015-cf5f6569d5a3', 19);");
        }
    }
}
