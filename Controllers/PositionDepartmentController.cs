using ColdTrack_Back.Authorization;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Repositories;
using ColdTrack_Back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColdTrack_Back.Controllers;

[ApiController]
[Route("[controller]")]
public class PositionDepartmentController(PositionDepartmentRepository positionDepartmentRepository) : ControllerBase
{
    [HttpPost]
    [HasPermission(Permissions.PositionUpdate)]
    public async Task<ActionResult> Assign([FromBody] AssignPositionDepartmentDto dto)
    {
        var record = await positionDepartmentRepository.Assign(dto.PositionId, dto.DepartmentId);
        if (record == null)
        {
            return BadRequest("职位或部门不存在，或关联已存在");
        }

        return Ok(record);
    }

    [HttpDelete]
    [HasPermission(Permissions.PositionUpdate)]
    public async Task<ActionResult> Remove([FromBody] AssignPositionDepartmentDto dto)
    {
        var ok = await positionDepartmentRepository.Remove(dto.PositionId, dto.DepartmentId);
        if (!ok)
        {
            return BadRequest("关联不存在");
        }

        return Ok();
    }

    [HttpGet]
    [Route("position/{id:long}/departments")]
    [HasPermission(Permissions.PositionRead)]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartmentsByPosition([FromRoute] long id)
    {
        return Ok(await positionDepartmentRepository.GetDepartmentsByPosition(id));
    }

    [HttpGet]
    [Route("department/{id}/positions")]
    [HasPermission(Permissions.PositionRead)]
    public async Task<ActionResult<IEnumerable<PositionDto>>> GetPositionsByDepartment([FromRoute] string id)
    {
        return Ok(await positionDepartmentRepository.GetPositionsByDepartment(id));
    }
}
