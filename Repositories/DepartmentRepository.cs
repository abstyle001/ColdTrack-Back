using ColdTrack_Back.Datas;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Models;
using ColdTrack_Back.Utils;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Repositories;

/*
 * 部门数据操作
 */
public class DepartmentRepository(ColdTrackDbContext db)
{
    // 创建一个部门
    public async Task<DepartmentDto?> CreateDepartment(CreateDepartmentDto dto)
    {
        // 生成部门id
        var seq = "";
        // 待生成部门的层级
        var level = dto.ParentId == null ? 1 : dto.ParentId.Length / 2 + 1;
        // 找出废弃部门表中最小的seq
        var free = await db.DiscardDepartments
            .Where(discardDepartment => discardDepartment.ParentId.Equals(dto.ParentId ?? string.Empty))
            .OrderBy(discardDepartment => discardDepartment.ChildId)
            .FirstOrDefaultAsync();
        if (free != null)
        {
            // 废弃部门表中有记录
            seq = FeelTheBaseUtil.DecimalToThirtyHexadecimal(free.ChildId);
            // 将此记录移出废弃部门表
            db.DiscardDepartments.Remove(free);
            await db.SaveChangesAsync();
        }
        else
        {
            // 废弃部门表中没有记录
            if (dto.ParentId == null || dto.ParentId.Equals(string.Empty))
            {
                // 寻找一级部门最大seq
                var departments = await db.Departments
                    .Where(x => x.Level == 1)
                    .ToListAsync();
                if (departments.Count == 0) seq = "00";
                else
                {
                    var maxId = departments.Max(d => 
                        FeelTheBaseUtil.ThirtyHexadecimalToDecimal(d.Id));
                    seq = FeelTheBaseUtil.DecimalToThirtyHexadecimal(maxId + 1);
                }
            }
            else
            {
                var department = await db.Departments.FindAsync(dto.ParentId);
                if (department == null)
                {
                    return null;
                }
                seq = department.MaxSeq;
                // 利用父部门的maxSeq找到最大的seq后，再对父部门的maxSeq进行更新
                department.MaxSeq = FeelTheBaseUtil.DecimalToThirtyHexadecimal(
                    FeelTheBaseUtil.ThirtyHexadecimalToDecimal(department.MaxSeq) + 1);
                db.Departments.Update(department);
                await db.SaveChangesAsync();
            }
        }
        var id = dto.ParentId == null ? string.Empty + seq : dto.ParentId + seq;
        // 生成部门记录
        var record = new Department
        {
            Id = id,
            Level = level,
            Name = dto.Name,
            ParentId = dto.ParentId == null || dto.ParentId.Equals(string.Empty) ? id : dto.ParentId,
            MaxSeq = "00",
            Explain = dto.Explain,
            ManagerId = dto.ManagerId ?? string.Empty,
            Workspace = dto.Workspace,
            Addition = dto.Addition,
        };
        // 插入新生成部门
        await db.Departments.AddAsync(record);
        await db.SaveChangesAsync();
        return new DepartmentDto
        {
            Id = record.Id,
            Name = record.Name,
            ParentId = record.ParentId,
            Level = record.Level,
            Explain = record.Explain,
            ManagerId = record.ManagerId,
            Workspace = record.Workspace,
            Addition = record.Addition,
            CreatedAt = record.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }

    public async Task<bool> ExistsDepartment(string id)
    {
        var department = await db.Departments.FindAsync(id);
        return department != null;
    }
}