using ColdTrack_Back.Datas;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Models;
using ColdTrack_Back.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Repositories;

/*
 * 用户数据操作
 */
public class UserRepository(
    UserManager<AppUser> userManager,
    ColdTrackDbContext db,
    IConfiguration config,
    IWebHostEnvironment env)
{
    // 获取所有管理员用户
    public async Task<HashSet<string>> GetAdminUsers()
    {
        var adminUsers = await userManager.GetUsersInRoleAsync(RoleType.Admin);
        return [..adminUsers.Select(u => u.Id)];
    }
    
    public UserDto? GetUserInfo(string id)
    {
        var user = db.Users.Find(id);
        if (user == null)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            NickName = user.NickName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Phone = user.PhoneNumber ?? string.Empty,
            City = user.City ?? string.Empty,
            CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            Avatar = user.Avatar
        };
    }

    public async Task<UserDto?> UpdateUser(string id, UpdateUserDto updateUserDto)
    {
        var user = await db.Users.FindAsync(id);
        if (user == null)
        {
            return null;
        }

        user.NickName = updateUserDto.NickName ?? user.NickName;
        user.City = updateUserDto.City ?? user.City;
        user.PhoneNumber = updateUserDto.Phone ?? user.PhoneNumber;
        var avatarUrl = await AvatarUtil.UploadAvatar(id, updateUserDto.File, config, env);
        if (avatarUrl != null)
        {
            user.Avatar = avatarUrl;
        }

        await db.SaveChangesAsync();
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            NickName = user.NickName ?? string.Empty,
            Phone = user.PhoneNumber ?? string.Empty,
            City = user.City ?? string.Empty,
            CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            Avatar = user.Avatar
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllUser()
    {
        var adminIds = await GetAdminUsers();
        return from user in db.Users.ToList()
            where !adminIds.Contains(user.Id)
            select new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                NickName = user.NickName,
                Phone = user.PhoneNumber,
                City = user.City,
                CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Avatar = user.Avatar
            };
    }

    public long GetUserCount()
    {
        return db.Users.Count();
    }

    public async Task<IEnumerable<UserDto>> GetUserPage(int pageNumber, int pageSize)
    {
        var userList = await db.Users
            .OrderBy(u => u.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var adminIds = await GetAdminUsers();
        return from user in userList
            where !adminIds.Contains(user.Id)
            select new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                NickName = user.NickName,
                Phone = user.PhoneNumber,
                City = user.City,
                CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Avatar = user.Avatar
            };
    }

    // 批量删除用户（根据传入的用户列表）
    public async Task DeleteUserBatch(List<string> ids)
    {
        // 清理用户-职位关联，避免孤儿数据
        await db.UserPositions
            .Where(up => ids.Contains(up.UserId))
            .ExecuteDeleteAsync();
        await db.Users
            .Where(u => ids.Contains(u.Id))
            .ExecuteDeleteAsync();
        await db.SaveChangesAsync();
    }
}