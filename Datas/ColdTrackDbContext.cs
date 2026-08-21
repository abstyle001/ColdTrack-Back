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
    public DbSet<TaskComment> TaskComments { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TaskTag> TaskTags { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // 数据库实际存在的角色 ID（Initial 迁移创建）：
        //   Admin = 417355cb-7f8b-4628-b6c9-c34af297ea67
        //   User  = a96a582b-2ab9-4528-8d45-b3a78f552e0f
        const string roleAdminId = "417355cb-7f8b-4628-b6c9-c34af297ea67";
        const string roleUserId = "a96a582b-2ab9-4528-8d45-b3a78f552e0f";

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

        builder.Entity<TaskComment>(entity =>
        {
            // 删除任务时级联删除其评论
            entity.HasOne(c => c.Task)
                  .WithMany()
                  .HasForeignKey(c => c.TaskId)
                  .OnDelete(DeleteBehavior.Cascade);

            // 删除用户时保留评论，作者置空
            entity.HasOne(c => c.Author)
                  .WithMany()
                  .HasForeignKey(c => c.AuthorId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(c => c.TaskId);
            entity.HasIndex(c => c.AuthorId);
        });

        builder.Entity<Tag>(entity =>
        {
            entity.HasIndex(t => t.Name).IsUnique();
        });

        builder.Entity<TaskTag>(entity =>
        {
            entity.HasKey(tt => new { tt.TaskId, tt.TagId });

            // 删除任务或标签时级联删除关联
            entity.HasOne(tt => tt.Task)
                  .WithMany(t => t.TaskTags)
                  .HasForeignKey(tt => tt.TaskId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(tt => tt.Tag)
                  .WithMany(t => t.TaskTags)
                  .HasForeignKey(tt => tt.TagId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(tt => tt.TagId);
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
            new() { Id = 19, Key = Perm.TaskComment, Name = "任务评论", Group = "任务管理" },
            new() { Id = 20, Key = Perm.TagRead, Name = "标签查看", Group = "任务管理" },
            new() { Id = 21, Key = Perm.TagCreate, Name = "标签创建", Group = "任务管理" },
            new() { Id = 22, Key = Perm.TagUpdate, Name = "标签编辑", Group = "任务管理" },
            new() { Id = 23, Key = Perm.TagDelete, Name = "标签删除", Group = "任务管理" },
        };
        builder.Entity<Permission>().HasData(permissions);

        // 角色-权限关联：Admin 拥有全部；User 拥有只读 + 自身编辑
        var rolePermissions = new List<RolePermission>();
        foreach (var p in permissions)
        {
            rolePermissions.Add(new RolePermission { RoleId = roleAdminId, PermissionId = p.Id });
        }
        foreach (var id in new[] { 1L, 3L, 6L, 10L, 14L, 19L, 20L })
        {
            rolePermissions.Add(new RolePermission { RoleId = roleUserId, PermissionId = id });
        }
        builder.Entity<RolePermission>().HasData(rolePermissions);
    }
}