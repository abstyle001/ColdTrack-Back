using ColdTrack_Back.Datas;
using ColdTrack_Back.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Services;

public class PermissionService(ColdTrackDbContext db) : IPermissionService
{
    public async Task<HashSet<string>> GetUserPermissionsAsync(string userId)
    {
        var roleIds = await db.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        if (roleIds.Count == 0)
        {
            return new HashSet<string>();
        }

        var keys = await db.RolePermissions
            .Where(rp => roleIds.Contains(rp.RoleId))
            .Join(db.Permissions,
                rp => rp.PermissionId,
                p => p.Id,
                (_, p) => p.Key)
            .ToListAsync();

        return new HashSet<string>(keys);
    }
}
