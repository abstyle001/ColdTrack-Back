using ColdTrack_Back.Datas;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Repositories;

public class TagRepository(ColdTrackDbContext db)
{
    public async Task<IEnumerable<TagDto>> GetAll()
        => await db.Tags
            .OrderBy(t => t.CreatedAt)
            .Select(t => ToDto(t))
            .ToListAsync();

    public async Task<TagDto> Create(CreateTagDto dto)
    {
        var tag = new Tag
        {
            Name = dto.Name.Trim(),
            Color = dto.Color,
            CreatedAt = DateTime.UtcNow
        };
        await db.Tags.AddAsync(tag);
        await db.SaveChangesAsync();
        return ToDto(tag);
    }

    public async Task<TagDto?> Update(long id, UpdateTagDto dto)
    {
        var tag = await db.Tags.FindAsync(id);
        if (tag == null) return null;
        if (dto.Name != null) tag.Name = dto.Name.Trim();
        if (dto.Color != null) tag.Color = dto.Color;
        await db.SaveChangesAsync();
        return ToDto(tag);
    }

    public async Task<bool> Delete(long id)
    {
        var tag = await db.Tags.FindAsync(id);
        if (tag == null) return false;
        // 显式清理关联，数据库级 Cascade 作为兜底
        await db.TaskTags.Where(tt => tt.TagId == id).ExecuteDeleteAsync();
        db.Tags.Remove(tag);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> NameExists(string name, long? exceptId = null)
        => await db.Tags.AnyAsync(t => t.Name == name && (!exceptId.HasValue || t.Id != exceptId.Value));

    private static TagDto ToDto(Tag t) => new()
    {
        Id = t.Id,
        Name = t.Name,
        Color = t.Color,
        CreatedAt = t.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
    };
}
