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
        var parentDepartment = await db.Departments.FindAsync(dto.ParentId);
        if (dto.ParentId != null && !dto.ParentId.Equals(string.Empty) && parentDepartment == null)
        {
            return null;
        }
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
            // 废弃表中取出的seq如果比当前maxSeq更大的话，要更新maxSeq
            if (parentDepartment != null && FeelTheBaseUtil.ThirtyHexadecimalToDecimal(parentDepartment.MaxSeq) <= free.ChildId)
            {
                parentDepartment.MaxSeq = FeelTheBaseUtil.DecimalToThirtyHexadecimal(free.ChildId + 1);
            }
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
                if (parentDepartment == null) return null;
                seq = parentDepartment.MaxSeq;
                // 利用父部门的maxSeq找到最大的seq后，再对父部门的maxSeq进行更新
                parentDepartment.MaxSeq = FeelTheBaseUtil.DecimalToThirtyHexadecimal(
                    FeelTheBaseUtil.ThirtyHexadecimalToDecimal(parentDepartment.MaxSeq) + 1);
                db.Departments.Update(parentDepartment);
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

    public async Task<DepartmentDto?> DeleteDepartment(string id)
    {
        var deletedDepartment = await db.Departments.FindAsync(id);
        if (deletedDepartment == null)
        {
            return null;
        }
        // 查出所有前缀为id的部门
        var departments = await db.Departments
            .Where(d => d.Id.StartsWith(id))
            .ToListAsync();
        // 执行前缀删除
        await db.Departments
            .Where(d => d.Id.StartsWith(id))
            .ExecuteDeleteAsync();
        // 将所有待删除部门添加至废弃部门表
        var discardDepartments = from department in departments
            select new DiscardDepartment
            {
                ParentId = department.ParentId.Equals(department.Id) ? string.Empty : department.ParentId,
                ChildId = FeelTheBaseUtil.ThirtyHexadecimalToDecimal(
                    department.Id.Length >= 2
                        ? department.Id[^2..]
                        : "")
            };
        // 将已删除的部门放入废弃部门表
        await db.DiscardDepartments.AddRangeAsync(discardDepartments);
        await db.SaveChangesAsync();
        // 返回指定删除的实体
        return new DepartmentDto
        {
            Id = deletedDepartment.Id,
            Name = deletedDepartment.Name,
            ParentId = deletedDepartment.ParentId,
            Level = deletedDepartment.Level,
            Explain = deletedDepartment.Explain,
            ManagerId = deletedDepartment.ManagerId,
            Workspace = deletedDepartment.Workspace,
            Addition = deletedDepartment.Addition,
            CreatedAt = deletedDepartment.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }

    public async Task<bool> ExistsDepartment(string id)
    {
        var department = await db.Departments.FindAsync(id);
        return department != null;
    }
}