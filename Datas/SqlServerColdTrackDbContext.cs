using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Datas;

/// <summary>
/// SQL Server 迁移用的设计时上下文。模型与基类完全一致，仅用于 dotnet ef 生成/应用
/// Migrations/SqlServer 目录下的迁移；运行时仍注入基类 ColdTrackDbContext。
/// </summary>
public class SqlServerColdTrackDbContext(DbContextOptions<SqlServerColdTrackDbContext> options)
    : ColdTrackDbContext(options)
{
}
