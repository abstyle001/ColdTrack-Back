namespace ColdTrack_Back.Utils;

/*
 * 权限键常量（资源:动作）。集中管理，供种子数据、授权特性、前端共用语义。
 */
public static class Permissions
{
    // 用户管理
    public const string UserRead = "user.read";
    public const string UserCreate = "user.create";
    public const string UserUpdate = "user.update";
    public const string UserDelete = "user.delete";
    public const string UserAssign = "user.assign";

    // 部门管理
    public const string DepartmentRead = "department.read";
    public const string DepartmentCreate = "department.create";
    public const string DepartmentUpdate = "department.update";
    public const string DepartmentDelete = "department.delete";

    // 职位管理
    public const string PositionRead = "position.read";
    public const string PositionCreate = "position.create";
    public const string PositionUpdate = "position.update";
    public const string PositionDelete = "position.delete";

    // 任务管理
    public const string TaskRead = "task.read";
    public const string TaskCreate = "task.create";
    public const string TaskUpdate = "task.update";
    public const string TaskDelete = "task.delete";
    public const string TaskComment = "task.comment";

    // 系统设置：角色与权限管理
    public const string RoleManage = "role.manage";

    // 超级管理员角色名
    public const string AdminRole = "Admin";
}
