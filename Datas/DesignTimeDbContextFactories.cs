using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ColdTrack_Back.Datas;

/// <summary>
/// dotnet ef 设计时工厂。连接字符串依次取 appsettings.json、
/// appsettings.Development.json、环境变量。用法：
///   dotnet ef migrations add &lt;Name&gt; --context SqlServerColdTrackDbContext -o Migrations/SqlServer
///   dotnet ef migrations add &lt;Name&gt; --context PostgresColdTrackDbContext -o Migrations/PostgreSql
/// </summary>
public class SqlServerColdTrackDbContextFactory : IDesignTimeDbContextFactory<SqlServerColdTrackDbContext>
{
    public SqlServerColdTrackDbContext CreateDbContext(string[] args)
    {
        var connectionString = DesignTimeDbContextFactories.BuildConfiguration()
            .GetConnectionString("DefaultConnection");
        var options = new DbContextOptionsBuilder<SqlServerColdTrackDbContext>()
            .UseSqlServer(connectionString)
            .Options;
        return new SqlServerColdTrackDbContext(options);
    }
}

/// <inheritdoc cref="SqlServerColdTrackDbContextFactory" />
public class PostgresColdTrackDbContextFactory : IDesignTimeDbContextFactory<PostgresColdTrackDbContext>
{
    public PostgresColdTrackDbContext CreateDbContext(string[] args)
    {
        // 设计时也要打开 legacy 时间戳开关，确保迁移生成 timestamp without time zone
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var connectionString = DesignTimeDbContextFactories.BuildConfiguration()
            .GetConnectionString("PostgresConnection");
        var options = new DbContextOptionsBuilder<PostgresColdTrackDbContext>()
            .UseNpgsql(connectionString)
            .Options;
        return new PostgresColdTrackDbContext(options);
    }
}

internal static class DesignTimeDbContextFactories
{
    public static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
}
