using ColdTrack_Back.Datas;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Repositories;

/*
 * 职位部门关系数据操作
 */
public class PositionDepartmentRepository(ColdTrackDbContext db)
{
    // 关联职位与部门
    public async Task<PositionDepartment?> Assign(long positionId, string departmentId)
    {
        if (!await db.Positions.AnyAsync(p => p.Id == positionId)) return null;
        if (!await db.Departments.AnyAsync(d => d.Id == departmentId)) return null;
        var exists = await db.PositionDepartments
            .AnyAsync(x => x.PositionId == positionId && x.DepartmentId == departmentId);
        if (exists) return null;
        var record = new PositionDepartment
        {
            PositionId = positionId,
            DepartmentId = departmentId
        };
        await db.PositionDepartments.AddAsync(record);
        await db.SaveChangesAsync();
        return record;
    }

    // 取消关联（按职位+部门）
    public async Task<bool> Remove(long positionId, string departmentId)
    {
        var rows = await db.PositionDepartments
            .Where(x => x.PositionId == positionId && x.DepartmentId == departmentId)
            .ExecuteDeleteAsync();
        return rows > 0;
    }

    // 按职位查部门
    public async Task<IEnumerable<DepartmentDto>> GetDepartmentsByPosition(long positionId)
    {
        return await db.PositionDepartments
            .Where(x => x.PositionId == positionId)
            .Join(db.Departments, pd => pd.DepartmentId, d => d.Id, (pd, d) => d)
            .OrderBy(d => d.Id)
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                ParentId = d.ParentId,
                Level = d.Level,
                Explain = d.Explain,
                ManagerId = d.ManagerId,
                ManagerName = db.Users.Where(u => u.Id == d.ManagerId).Select(u => u.NickName).FirstOrDefault(),
                Workspace = d.Workspace,
                Addition = d.Addition,
                CreatedAt = d.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            })
            .ToListAsync();
    }

    // 按部门查职位
    public async Task<IEnumerable<PositionDto>> GetPositionsByDepartment(string departmentId)
    {
        return await db.PositionDepartments
            .Where(x => x.DepartmentId == departmentId)
            .Join(db.Positions, pd => pd.PositionId, p => p.Id, (pd, p) => p)
            .OrderBy(p => p.CreatedAt)
            .Select(p => new PositionDto
            {
                Id = p.Id,
                Name = p.Name,
                Duty = p.Duty,
                Workspace = p.Workspace,
                Addition = p.Addition,
                CreatedAt = p.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            })
            .ToListAsync();
    }

    public async Task RemoveByPosition(long positionId)
    {
        await db.PositionDepartments
            .Where(x => x.PositionId == positionId)
            .ExecuteDeleteAsync();
    }

    public async Task RemoveByDepartment(string departmentId)
    {
        await db.PositionDepartments
            .Where(x => x.DepartmentId == departmentId)
            .ExecuteDeleteAsync();
    }
}
