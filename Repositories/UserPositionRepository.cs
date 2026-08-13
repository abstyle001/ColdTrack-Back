using ColdTrack_Back.Datas;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Repositories;

/*
 * 用户职位关系数据操作
 */
public class UserPositionRepository(ColdTrackDbContext db)
{
    // 分配用户到职位
    public async Task<UserPosition?> Assign(string userId, long positionId)
    {
        if (!await db.Users.AnyAsync(u => u.Id == userId)) return null;
        if (!await db.Positions.AnyAsync(p => p.Id == positionId)) return null;
        var exists = await db.UserPositions
            .AnyAsync(x => x.UserId == userId && x.PositionId == positionId);
        if (exists) return null;
        var record = new UserPosition
        {
            UserId = userId,
            PositionId = positionId
        };
        await db.UserPositions.AddAsync(record);
        await db.SaveChangesAsync();
        return record;
    }

    // 取消分配（按用户+职位）
    public async Task<bool> Remove(string userId, long positionId)
    {
        var rows = await db.UserPositions
            .Where(x => x.UserId == userId && x.PositionId == positionId)
            .ExecuteDeleteAsync();
        return rows > 0;
    }

    // 按用户查职位+部门聚合
    public async Task<List<UserPositionViewDto>> GetUserPositionsWithDepartments(string userId)
    {
        var userPositions = await db.UserPositions
            .Where(up => up.UserId == userId)
            .ToListAsync();
        if (userPositions.Count == 0) return [];
        var positionIds = userPositions.Select(up => up.PositionId).Distinct().ToList();
        var positions = await db.Positions
            .Where(p => positionIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);
        var pdMap = await db.PositionDepartments
            .Where(pd => positionIds.Contains(pd.PositionId))
            .ToListAsync();
        var deptIds = pdMap.Select(pd => pd.DepartmentId).Distinct().ToList();
        var deptDict = await db.Departments
            .Where(d => deptIds.Contains(d.Id))
            .ToDictionaryAsync(d => d.Id);

        var result = new List<UserPositionViewDto>();
        foreach (var up in userPositions)
        {
            positions.TryGetValue(up.PositionId, out var pos);
            var depts = pdMap.Where(pd => pd.PositionId == up.PositionId).ToList();
            if (depts.Count == 0)
            {
                result.Add(new UserPositionViewDto
                {
                    PositionId = up.PositionId,
                    PositionName = pos?.Name ?? string.Empty,
                    PositionDuty = pos?.Duty
                });
            }
            else
            {
                foreach (var pd in depts)
                {
                    deptDict.TryGetValue(pd.DepartmentId, out var dept);
                    result.Add(new UserPositionViewDto
                    {
                        PositionId = up.PositionId,
                        PositionName = pos?.Name ?? string.Empty,
                        PositionDuty = pos?.Duty,
                        DepartmentId = pd.DepartmentId,
                        DepartmentName = dept?.Name
                    });
                }
            }
        }
        return result;
    }

    // 按职位查用户
    public async Task<IEnumerable<UserDto>> GetUsersByPosition(long positionId)
    {
        var userIds = await db.UserPositions
            .Where(up => up.PositionId == positionId)
            .Select(up => up.UserId)
            .Distinct()
            .ToListAsync();
        return await db.Users
            .Where(u => userIds.Contains(u.Id))
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName ?? string.Empty,
                Email = u.Email ?? string.Empty,
                NickName = u.NickName ?? string.Empty,
                Phone = u.PhoneNumber,
                City = u.City,
                CreatedAt = u.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Avatar = u.Avatar
            })
            .ToListAsync();
    }

    public async Task RemoveByUser(string userId)
    {
        await db.UserPositions
            .Where(x => x.UserId == userId)
            .ExecuteDeleteAsync();
    }

    public async Task RemoveByPosition(long positionId)
    {
        await db.UserPositions
            .Where(x => x.PositionId == positionId)
            .ExecuteDeleteAsync();
    }
}
