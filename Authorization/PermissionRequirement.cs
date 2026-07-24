using Microsoft.AspNetCore.Authorization;

namespace ColdTrack_Back.Authorization;

/*
 * 权限要求：携带所需权限键（如 "user.delete"）。
 * 与 PermissionAuthorizationHandler 配合，在每次请求时动态校验。
 */
public class PermissionRequirement : IAuthorizationRequirement
{
    public string PermissionKey { get; }

    public PermissionRequirement(string permissionKey)
    {
        PermissionKey = permissionKey;
    }
}
