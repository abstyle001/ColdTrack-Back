using ColdTrack_Back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Datas;

public class ColdTrackDbContext(DbContextOptions<ColdTrackDbContext> options) : IdentityDbContext<AppUser, IdentityRole, string>(options)
{
    public DbSet<Position> Positions { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<UserPosition> UserPositions { get; set; }
    public DbSet<PositionDepartment> PositionDepartments { get; set; }
    public DbSet<DiscardDepartment> DiscardDepartments { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<AppUser>(entity =>
        {
            entity.Property(u => u.CreatedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");
        });
        var roles = new List<IdentityRole>
        {
            new()
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new()
            {
                Name = "User",
                NormalizedName = "USER"
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);
    }
}