using ColdTrack_Back.Datas;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Repositories;

/*
 * 职位数据操作
 */
public class PositionRepository(ColdTrackDbContext db)
{
    public async Task<IEnumerable<PositionDto>> GetAll()
    {
        return await db.Positions
            .OrderBy(p => p.CreatedAt)
            .Select(p => ToDto(p))
            .ToListAsync();
    }

    public async Task<PositionDto?> GetById(long id)
    {
        var position = await db.Positions.FindAsync(id);
        return position == null ? null : ToDto(position);
    }

    public async Task<PositionDto> Create(CreatePositionDto dto)
    {
        var position = new Position
        {
            Name = dto.Name,
            Duty = dto.Duty,
            Workspace = dto.Workspace,
            Addition = dto.Addition,
            CreatedAt = DateTime.UtcNow
        };
        await db.Positions.AddAsync(position);
        await db.SaveChangesAsync();
        return ToDto(position);
    }

    public async Task<PositionDto?> Update(long id, UpdatePositionDto dto)
    {
        var position = await db.Positions.FindAsync(id);
        if (position == null) return null;
        position.Name = dto.Name ?? position.Name;
        position.Duty = dto.Duty ?? position.Duty;
        position.Workspace = dto.Workspace ?? position.Workspace;
        position.Addition = dto.Addition ?? position.Addition;
        await db.SaveChangesAsync();
        return ToDto(position);
    }

    public async Task<bool> Delete(long id)
    {
        var position = await db.Positions.FindAsync(id);
        if (position == null) return false;
        // 级联清理关联关系
        await db.PositionDepartments.Where(x => x.PositionId == id).ExecuteDeleteAsync();
        await db.UserPositions.Where(x => x.PositionId == id).ExecuteDeleteAsync();
        db.Positions.Remove(position);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Exists(long id)
    {
        return await db.Positions.AnyAsync(p => p.Id == id);
    }

    private static PositionDto ToDto(Position p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Duty = p.Duty,
        Workspace = p.Workspace,
        Addition = p.Addition,
        CreatedAt = p.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
    };
}
