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
        return await GetById(record.Id);
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
        return await db.Departments.AnyAsync(d => d.Id == id);
    }

    // 获取全部部门（扁平，含负责人昵称）
    public async Task<IEnumerable<DepartmentDto>> GetAll()
    {
        return await db.Departments
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

    // 获取单个部门详情（含负责人昵称）
    public async Task<DepartmentDto?> GetById(string id)
    {
        var department = await db.Departments.FindAsync(id);
        if (department == null) return null;
        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            ParentId = department.ParentId,
            Level = department.Level,
            Explain = department.Explain,
            ManagerId = department.ManagerId,
            ManagerName = await db.Users
                .Where(u => u.Id == department.ManagerId)
                .Select(u => u.NickName)
                .FirstOrDefaultAsync(),
            Workspace = department.Workspace,
            Addition = department.Addition,
            CreatedAt = department.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }

    // 获取部门树（层级结构）
    public async Task<IEnumerable<DepartmentTreeDto>> GetTree()
    {
        var all = await GetAll();
        var dict = all.ToDictionary(d => d.Id, d => new DepartmentTreeDto
        {
            Id = d.Id,
            Name = d.Name,
            ParentId = d.ParentId,
            Level = d.Level,
            Explain = d.Explain,
            ManagerId = d.ManagerId,
            ManagerName = d.ManagerName,
            Workspace = d.Workspace,
            Addition = d.Addition,
            CreatedAt = d.CreatedAt
        });
        var roots = new List<DepartmentTreeDto>();
        foreach (var node in dict.Values)
        {
            if (dict.TryGetValue(node.ParentId, out var parent))
            {
                parent.Children.Add(node);
            }
            else
            {
                roots.Add(node);
            }
        }
        return roots;
    }

    // 更新部门
    public async Task<DepartmentDto?> Update(string id, DepartmentDto dto)
    {
        var department = await db.Departments.FindAsync(id);
        if (department == null) return null;
        department.Name = dto.Name;
        department.Explain = dto.Explain;
        department.Workspace = dto.Workspace;
        department.ManagerId = dto.ManagerId ?? string.Empty;
        department.Addition = dto.Addition;
        await db.SaveChangesAsync();
        return await GetById(id);
    }
}
