using System.Security.Claims;
using ColdTrack_Back.Services;
using Microsoft.AspNetCore.Authorization;

namespace ColdTrack_Back.Authorization;

/*
 * 动态权限处理器：从缓存/数据库实时解析用户权限集合，命中即放行。
 * 不依赖 JWT 中的角色声明，因此管理员修改权限后无需重新登录即可生效。
 */
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionCacheService _cacheService;

    public PermissionAuthorizationHandler(IPermissionCacheService cacheService)
    {
        _cacheService = cacheService;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userId = context.User.FindFirstValue("id")
                     ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? context.User.FindFirstValue("sub");
        if (userId == null)
        {
            return;
        }

        var permissions = await _cacheService.GetPermissionsAsync(userId);
        if (permissions.Contains(requirement.PermissionKey))
        {
            context.Succeed(requirement);
        }
    }
}
