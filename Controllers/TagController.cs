using ColdTrack_Back.Authorization;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Repositories;
using ColdTrack_Back.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ColdTrack_Back.Controllers;

[ApiController]
[Route("[controller]")]
public class TagController(TagRepository tagRepository) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.TagRead)]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetAll()
        => Ok(await tagRepository.GetAll());

    [HttpPost]
    [HasPermission(Permissions.TagCreate)]
    public async Task<ActionResult<TagDto>> Create([FromBody] CreateTagDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("标签名称不能为空");
        if (await tagRepository.NameExists(dto.Name.Trim()))
            return BadRequest("标签名称已存在");
        return Ok(await tagRepository.Create(dto));
    }

    [HttpPut]
    [Route("{id:long}")]
    [HasPermission(Permissions.TagUpdate)]
    public async Task<ActionResult<TagDto>> Update([FromRoute] long id, [FromBody] UpdateTagDto dto)
    {
        if (dto.Name != null && string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("标签名称不能为空");
        if (dto.Name != null && await tagRepository.NameExists(dto.Name.Trim(), id))
            return BadRequest("标签名称已存在");
        var tag = await tagRepository.Update(id, dto);
        if (tag == null)
            return BadRequest("标签不存在");
        return Ok(tag);
    }

    [HttpDelete]
    [Route("{id:long}")]
    [HasPermission(Permissions.TagDelete)]
    public async Task<ActionResult> Delete([FromRoute] long id)
    {
        var ok = await tagRepository.Delete(id);
        if (!ok)
            return BadRequest("标签不存在");
        return Ok();
    }
}
