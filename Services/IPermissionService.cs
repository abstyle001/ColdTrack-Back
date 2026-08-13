namespace ColdTrack_Back.Services;

/*
 * 解析用户的有效权限集合（按角色聚合）。数据源为数据库，保证动态。
 */
public interface IPermissionService
{
    Task<HashSet<string>> GetUserPermissionsAsync(string userId);
}
