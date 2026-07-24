using ColdTrack_Back.Authorization;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Repositories;
using ColdTrack_Back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColdTrack_Back.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController(DepartmentRepository departmentRepository) : ControllerBase
{
    [HttpPost]
    [HasPermission(Permissions.DepartmentCreate)]
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

    [HttpDelete]
    [Route("{id}")]
    [HasPermission(Permissions.DepartmentDelete)]
    public async Task<ActionResult<DepartmentDto>> DeleteDepartment([FromRoute] string id)
    {
        var department = await departmentRepository.DeleteDepartment(id);
        if (department == null)
        {
            return BadRequest("所删除的部门不存在");
        }

        return Ok(department);
    }

    [HttpGet]
    [HasPermission(Permissions.DepartmentRead)]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllDepartments()
    {
        return Ok(await departmentRepository.GetAll());
    }

    [HttpGet]
    [Route("page")]
    [HasPermission(Permissions.DepartmentRead)]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartmentPage([FromQuery] int number, [FromQuery] int size)
    {
        return Ok(await departmentRepository.GetPage(number, size));
    }

    [HttpGet]
    [Route("count")]
    [HasPermission(Permissions.DepartmentRead)]
    public async Task<ActionResult<long>> GetDepartmentCount()
    {
        return Ok(await departmentRepository.GetTopLevelCount());
    }

    [HttpGet]
    [Route("tree")]
    [HasPermission(Permissions.DepartmentRead)]
    public async Task<ActionResult<IEnumerable<DepartmentTreeDto>>> GetDepartmentTree()
    {
        return Ok(await departmentRepository.GetTree());
    }

    [HttpGet]
    [Route("{id}")]
    [HasPermission(Permissions.DepartmentRead)]
    public async Task<ActionResult<DepartmentDto>> GetDepartment([FromRoute] string id)
    {
        var department = await departmentRepository.GetById(id);
        if (department == null)
        {
            return NotFound("部门不存在");
        }

        return Ok(department);
    }

    [HttpPut]
    [Route("{id}")]
    [HasPermission(Permissions.DepartmentUpdate)]
    public async Task<ActionResult<DepartmentDto>> UpdateDepartment([FromRoute] string id, [FromBody] DepartmentDto dto)
    {
        var department = await departmentRepository.Update(id, dto);
        if (department == null)
        {
            return BadRequest("部门不存在");
        }

        return Ok(department);
    }
}
