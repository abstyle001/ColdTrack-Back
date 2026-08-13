using Microsoft.AspNetCore.Authorization;

namespace ColdTrack_Back.Authorization;

/*
 * 便捷特性：直接在 Controller/Action 上标注所需权限。
 * 例：[HasPermission(Permissions.UserDelete)]
 */
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
    {
        Policy = "perm:" + permission;
    }
}
