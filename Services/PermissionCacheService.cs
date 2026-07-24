using Microsoft.Extensions.Caching.Memory;

namespace ColdTrack_Back.Services;

public class PermissionCacheService(
    IMemoryCache cache,
    IPermissionService permissionService) : IPermissionCacheService
{
    private const string GlobalVersionKey = "perm:global-version";
    private static string UserVersionKey(string userId) => $"perm:user-version:{userId}";

    public async Task<HashSet<string>> GetPermissionsAsync(string userId)
    {
        var gv = GetOrCreateLong(GlobalVersionKey);
        var uv = GetOrCreateLong(UserVersionKey(userId));
        var key = $"perm:set:{userId}:{gv}:{uv}";

        var result = await cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(5);
            return await permissionService.GetUserPermissionsAsync(userId);
        });

        return result ?? new HashSet<string>();
    }

    public void InvalidateUser(string userId)
    {
        Bump(UserVersionKey(userId));
    }

    public void InvalidateAll()
    {
        Bump(GlobalVersionKey);
    }

    private long GetOrCreateLong(string key)
    {
        return cache.GetOrCreate(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(365);
            return 0L;
        });
    }

    private void Bump(string key)
    {
        var value = GetOrCreateLong(key) + 1;
        cache.Set(key, value, TimeSpan.FromDays(365));
    }
}
