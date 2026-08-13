using ColdTrack_Back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Perm = ColdTrack_Back.Utils.Permissions;

namespace ColdTrack_Back.Datas;

public class ColdTrackDbContext(DbContextOptions<ColdTrackDbContext> options) : IdentityDbContext<AppUser, IdentityRole, string>(options)
{
    public DbSet<Position> Positions { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<UserPosition> UserPositions { get; set; }
    public DbSet<PositionDepartment> PositionDepartments { get; set; }
    public DbSet<DiscardDepartment> DiscardDepartments { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // 固定种子 ID，与数据库实际角色 ID 对齐
        const string roleAdminId = "f0874ebd-be74-4a5e-951a-c738f27f6cb8";
        const string roleUserId = "7d06dcab-5a11-482e-b015-cf5f6569d5a3";

        builder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder.Entity<TaskItem>(entity =>
        {
            entity.HasOne(t => t.Assignee)
                  .WithMany()
                  .HasForeignKey(t => t.AssigneeId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(t => t.Creator)
                  .WithMany()
                  .HasForeignKey(t => t.CreatorId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(t => t.AssigneeId);
            entity.HasIndex(t => t.CreatorId);
            entity.HasIndex(t => t.Status);
        });

        // 权限目录种子
        var permissions = new List<Permission>
        {
            new() { Id = 1, Key = Perm.UserRead, Name = "用户查看", Group = "用户管理" },
            new() { Id = 2, Key = Perm.UserCreate, Name = "用户创建", Group = "用户管理" },
            new() { Id = 3, Key = Perm.UserUpdate, Name = "用户编辑", Group = "用户管理" },
            new() { Id = 4, Key = Perm.UserDelete, Name = "用户删除", Group = "用户管理" },
            new() { Id = 5, Key = Perm.UserAssign, Name = "用户分配职位", Group = "用户管理" },
            new() { Id = 6, Key = Perm.DepartmentRead, Name = "部门查看", Group = "部门管理" },
            new() { Id = 7, Key = Perm.DepartmentCreate, Name = "部门创建", Group = "部门管理" },
            new() { Id = 8, Key = Perm.DepartmentUpdate, Name = "部门编辑", Group = "部门管理" },
            new() { Id = 9, Key = Perm.DepartmentDelete, Name = "部门删除", Group = "部门管理" },
            new() { Id = 10, Key = Perm.PositionRead, Name = "职位查看", Group = "职位管理" },
            new() { Id = 11, Key = Perm.PositionCreate, Name = "职位创建", Group = "职位管理" },
            new() { Id = 12, Key = Perm.PositionUpdate, Name = "职位编辑", Group = "职位管理" },
            new() { Id = 13, Key = Perm.PositionDelete, Name = "职位删除", Group = "职位管理" },
            new() { Id = 14, Key = Perm.TaskRead, Name = "任务查看", Group = "任务管理" },
            new() { Id = 15, Key = Perm.TaskCreate, Name = "任务创建", Group = "任务管理" },
            new() { Id = 16, Key = Perm.TaskUpdate, Name = "任务编辑", Group = "任务管理" },
            new() { Id = 17, Key = Perm.TaskDelete, Name = "任务删除", Group = "任务管理" },
            new() { Id = 18, Key = Perm.RoleManage, Name = "角色与权限管理", Group = "系统设置" },
        };
        builder.Entity<Permission>().HasData(permissions);

        // 角色-权限关联：Admin 拥有全部；User 拥有只读 + 自身编辑
        var rolePermissions = new List<RolePermission>();
        foreach (var p in permissions)
        {
            rolePermissions.Add(new RolePermission { RoleId = roleAdminId, PermissionId = p.Id });
        }
        foreach (var id in new[] { 1L, 3L, 6L, 10L, 14L })
        {
            rolePermissions.Add(new RolePermission { RoleId = roleUserId, PermissionId = id });
        }
        builder.Entity<RolePermission>().HasData(rolePermissions);
    }
}