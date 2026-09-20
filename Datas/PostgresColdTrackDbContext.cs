using ColdTrack_Back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Datas;

/// <summary>
/// PostgreSQL 迁移用的设计时上下文。模型在基类之上额外携带种子数据（角色、默认管理员、
/// 用户-角色关联），仅用于 dotnet ef 生成/应用 Migrations/PostgreSql 目录下的迁移；
/// 运行时仍注入基类 ColdTrackDbContext。
/// </summary>
public class PostgresColdTrackDbContext(DbContextOptions<PostgresColdTrackDbContext> options)
    : ColdTrackDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // 与 SQL Server 侧 Initial 迁移的 InsertData 保持一致，PG 全新初始化时重建相同种子
        const string roleAdminId = "417355cb-7f8b-4628-b6c9-c34af297ea67";
        const string roleUserId = "a96a582b-2ab9-4528-8d45-b3a78f552e0f";
        const string adminUserId = "de4d5418-88ef-4d01-80a4-dfc971ae3d47";

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = roleAdminId, Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = roleUserId, Name = "User", NormalizedName = "USER" });

        builder.Entity<AppUser>().HasData(new AppUser
        {
            Id = adminUserId,
            UserName = "admin@cold.com",
            NormalizedUserName = "ADMIN@COLD.COM",
            Email = "admin@cold.com",
            NormalizedEmail = "ADMIN@COLD.COM",
            EmailConfirmed = false,
            PasswordHash = "AQAAAAIAAYagAAAAELUl/KphdKnCALf14U8IuxYFjhP9bK0HguKM2DlGh5bT/RGf17rpOiH4f7FnpzidgA==",
            SecurityStamp = "c25b3ec0-5b13-4e4a-8238-05a87542ada6",
            ConcurrencyStamp = "1dbe8e32-024a-4d48-9423-000b22811b2f",
            PhoneNumber = "17323895436",
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = true,
            AccessFailedCount = 0,
            NickName = "Admin",
            City = "北京市",
            CreatedAt = new DateTime(2026, 4, 28, 16, 14, 18, 972, DateTimeKind.Utc).AddTicks(5242)
        });

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { RoleId = roleAdminId, UserId = adminUserId });
    }
}
