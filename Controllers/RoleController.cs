using ColdTrack_Back.Authorization;
using ColdTrack_Back.Datas;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Models;
using ColdTrack_Back.Services;
using ColdTrack_Back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Controllers;

[ApiController]
[Route("role")]
[HasPermission(Permissions.RoleManage)]
public class RoleController(
    ColdTrackDbContext db,
    RoleManager<IdentityRole> roleManager,
    UserManager<AppUser> userManager,
    IPermissionCacheService permissionCache) : ControllerBase
{
    // 权限目录全量
    [HttpGet("permissions")]
    public async Task<ActionResult<List<PermissionDto>>> GetPermissions()
    {
        var list = await db.Permissions
            .OrderBy(p => p.Group).ThenBy(p => p.Id)
            .Select(p => new PermissionDto
            {
                Id = p.Id,
                Key = p.Key,
                Name = p.Name,
                Group = p.Group,
                Description = p.Description
            })
            .ToListAsync();
        return Ok(list);
    }

    // 角色列表（含各自权限）
    [HttpGet]
    public async Task<ActionResult<List<RoleDto>>> GetRoles()
    {
        var roles = roleManager.Roles.ToList();
        var result = new List<RoleDto>();
        foreach (var role in roles)
        {
            var permissionIds = await db.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => rp.PermissionId)
                .ToListAsync();
            var perms = await db.Permissions
                .Where(p => permissionIds.Contains(p.Id))
                .OrderBy(p => p.Group).ThenBy(p => p.Id)
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Key = p.Key,
                    Name = p.Name,
                    Group = p.Group,
                    Description = p.Description
                })
                .ToListAsync();

            result.Add(new RoleDto
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty,
                Permissions = perms
            });
        }

        return Ok(result);
    }

    // 新建自定义角色
    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("角色名不能为空");
        }

        if (await roleManager.RoleExistsAsync(dto.Name))
        {
            return BadRequest("角色已存在");
        }

        var role = new IdentityRole(dto.Name) { NormalizedName = dto.Name.ToUpperInvariant() };
        var create = await roleManager.CreateAsync(role);
        if (!create.Succeeded)
        {
            return StatusCode(500, create.Errors);
        }

        permissionCache.InvalidateAll();
        return Ok(new RoleDto { Id = role.Id, Name = role.Name ?? string.Empty });
    }

    // 设置某角色的权限（全量替换）
    [HttpPut("{roleId}/permissions")]
    public async Task<ActionResult> UpdateRolePermissions(
        [FromRoute] string roleId,
        [FromBody] AssignRolePermissionsDto dto)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            return NotFound("角色不存在");
        }

        var existing = db.RolePermissions.Where(rp => rp.RoleId == roleId);
        db.RolePermissions.RemoveRange(existing);

        var ids = await db.Permissions
            .Where(p => dto.PermissionKeys.Contains(p.Key))
            .Select(p => p.Id)
            .ToListAsync();

        foreach (var id in ids)
        {
            db.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = id });
        }

        await db.SaveChangesAsync();
        permissionCache.InvalidateAll();
        return Ok();
    }

    // 用户加入角色
    [HttpPost("{roleId}/users/{userId}")]
    public async Task<ActionResult> AddUserToRole([FromRoute] string roleId, [FromRoute] string userId)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        var user = await userManager.FindByIdAsync(userId);
        if (role == null || user == null)
        {
            return NotFound("角色或用户不存在");
        }

        var result = await userManager.AddToRoleAsync(user, role.Name!);
        if (!result.Succeeded)
        {
            return StatusCode(500, result.Errors);
        }

        permissionCache.InvalidateUser(userId);
        return Ok();
    }

    // 用户移出角色
    [HttpDelete("{roleId}/users/{userId}")]
    public async Task<ActionResult> RemoveUserFromRole([FromRoute] string roleId, [FromRoute] string userId)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        var user = await userManager.FindByIdAsync(userId);
        if (role == null || user == null)
        {
            return NotFound("角色或用户不存在");
        }

        var result = await userManager.RemoveFromRoleAsync(user, role.Name!);
        if (!result.Succeeded)
        {
            return StatusCode(500, result.Errors);
        }

        permissionCache.InvalidateUser(userId);
        return Ok();
    }

    // 获取某角色的所有用户成员
    [HttpGet("{roleId}/users")]
    public async Task<ActionResult<List<UserDto>>> GetRoleUsers([FromRoute] string roleId)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            return NotFound("角色不存在");
        }

        var users = await db.UserRoles
            .Where(ur => ur.RoleId == roleId)
            .Join(db.Users, ur => ur.UserId, u => u.Id, (_, u) => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName ?? string.Empty,
                Email = u.Email ?? string.Empty,
                NickName = u.NickName ?? string.Empty,
                Phone = u.PhoneNumber ?? string.Empty,
                City = u.City ?? string.Empty,
                CreatedAt = u.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Avatar = u.Avatar
            })
            .ToListAsync();

        return Ok(users);
    }

    // 获取某用户的所有角色
    [HttpGet("user/{userId}/roles")]
    public async Task<ActionResult<List<RoleDto>>> GetUserRoles([FromRoute] string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("用户不存在");
        }

        var roleNames = await userManager.GetRolesAsync(user);
        var roles = roleManager.Roles
            .Where(r => roleNames.Contains(r.Name!))
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name ?? string.Empty,
                Permissions = new List<PermissionDto>()
            })
            .ToList();

        return Ok(roles);
    }
}
