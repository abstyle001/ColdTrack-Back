using ColdTrack_Back.Authorization;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Repositories;
using ColdTrack_Back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColdTrack_Back.Controllers;

[ApiController]
[Route("[controller]")]
public class PositionController(PositionRepository positionRepository) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.PositionRead)]
    public async Task<ActionResult<IEnumerable<PositionDto>>> GetAll()
    {
        return Ok(await positionRepository.GetAll());
    }

    [HttpGet]
    [Route("{id:long}")]
    [HasPermission(Permissions.PositionRead)]
    public async Task<ActionResult<PositionDto>> GetById([FromRoute] long id)
    {
        var position = await positionRepository.GetById(id);
        if (position == null)
        {
            return NotFound("职位不存在");
        }

        return Ok(position);
    }

    [HttpGet]
    [Route("page")]
    [HasPermission(Permissions.PositionRead)]
    public async Task<ActionResult<IEnumerable<PositionDto>>> GetPositionPage([FromQuery] int number, [FromQuery] int size)
    {
        return Ok(await positionRepository.GetPage(number, size));
    }

    [HttpGet]
    [Route("count")]
    [HasPermission(Permissions.PositionRead)]
    public ActionResult<long> GetPositionCount()
    {
        return Ok(positionRepository.GetCount());
    }

    [HttpPost]
    [HasPermission(Permissions.PositionCreate)]
    public async Task<ActionResult<PositionDto>> Create([FromBody] CreatePositionDto dto)
    {
        var position = await positionRepository.Create(dto);
        return Ok(position);
    }

    [HttpPut]
    [Route("{id:long}")]
    [HasPermission(Permissions.PositionUpdate)]
    public async Task<ActionResult<PositionDto>> Update([FromRoute] long id, [FromBody] UpdatePositionDto dto)
    {
        var position = await positionRepository.Update(id, dto);
        if (position == null)
        {
            return BadRequest("职位不存在");
        }

        return Ok(position);
    }

    [HttpDelete]
    [Route("{id:long}")]
    [HasPermission(Permissions.PositionDelete)]
    public async Task<ActionResult> Delete([FromRoute] long id)
    {
        var ok = await positionRepository.Delete(id);
        if (!ok)
        {
            return BadRequest("职位不存在");
        }

        return Ok();
    }
}
