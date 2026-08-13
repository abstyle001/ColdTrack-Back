namespace ColdTrack_Back.Services;

/*
 * 带缓存的用户权限解析。通过全局版本号 + 用户版本号实现即时失效，
 * 无需重新登录即可让权限变更生效。兜底 5 分钟滑动过期防止缓存穿透。
 */
public interface IPermissionCacheService
{
    Task<HashSet<string>> GetPermissionsAsync(string userId);
    void InvalidateUser(string userId);
    void InvalidateAll();
}
