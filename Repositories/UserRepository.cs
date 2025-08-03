using ColdTrack_Back.Datas;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Utils;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Repositories;

public class UserRepository(ColdTrackDbContext db, IConfiguration config, IWebHostEnvironment env)
{
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
            Avatar = avatarUrl ?? string.Empty
        };
    }

    public IEnumerable<UserDto> GetAllUser()
    {
        return from user in db.Users.ToList()
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
        return from user in userList
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
}