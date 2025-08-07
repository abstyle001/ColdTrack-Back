using ColdTrack_Back.Dtos;
using ColdTrack_Back.Repositories;
using ColdTrack_Back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ColdTrack_Back.Controllers;

[ApiController]
[Route("department")]
public class DepartmentController(DepartmentRepository departmentRepository) : ControllerBase
{
    [HttpPost]
    // [Authorize(Roles = RoleType.Admin)]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] CreateDepartmentDto dto)
    {
        if (dto.ParentId != null && !dto.ParentId.Equals(string.Empty))
        {
            if (!await departmentRepository.ExistsDepartment(dto.ParentId))
            {
                return BadRequest("该创建部门的所属部门不存在");
            }
        }
        var departmentDto = await departmentRepository.CreateDepartment(dto);
        if (departmentDto == null)
        {
            return StatusCode(500, "服务器错误，请稍后再试");
        }

        return Ok(departmentDto);
    }
}