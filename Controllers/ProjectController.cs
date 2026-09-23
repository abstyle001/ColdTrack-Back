using System.Security.Claims;
using ColdTrack_Back.Authorization;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Repositories;
using ColdTrack_Back.Services;
using ColdTrack_Back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColdTrack_Back.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectController(ProjectRepository projectRepository, IPermissionCacheService cacheService) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ProjectRead)]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll(
        [FromQuery] string? status = null,
        [FromQuery] string? managerId = null,
        [FromQuery] string? keyword = null)
        => Ok(await projectRepository.GetAll(status, managerId, keyword));

    [HttpGet]
    [Route("{id:long}")]
    [HasPermission(Permissions.ProjectRead)]
    public async Task<ActionResult<ProjectDto>> GetById([FromRoute] long id)
    {
        var project = await projectRepository.GetById(id);
        if (project == null)
            return NotFound("项目不存在");
        return Ok(project);
    }

    [HttpPost]
    [HasPermission(Permissions.ProjectCreate)]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            return Ok(await projectRepository.Create(dto));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    // 手动鉴权：拥有 project.update 权限，或该项目的负责人
    [HttpPut]
    [Route("{id:long}")]
    [Authorize]
    public async Task<ActionResult<ProjectDto>> Update([FromRoute] long id, [FromBody] UpdateProjectDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub")
                     ?? User.FindFirstValue("id");
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("无法获取用户身份");
        var perms = await cacheService.GetPermissionsAsync(userId);
        if (!perms.Contains(Permissions.ProjectUpdate) && !await projectRepository.IsManager(id, userId))
            return Forbid();
        try
        {
            var project = await projectRepository.Update(id, dto);
            if (project == null)
                return NotFound("项目不存在");
            return Ok(project);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    // 手动鉴权：拥有 project.delete 权限，或该项目的负责人
    [HttpDelete]
    [Route("{id:long}")]
    [Authorize]
    public async Task<ActionResult> Delete([FromRoute] long id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub")
                     ?? User.FindFirstValue("id");
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("无法获取用户身份");
        var perms = await cacheService.GetPermissionsAsync(userId);
        if (!perms.Contains(Permissions.ProjectDelete) && !await projectRepository.IsManager(id, userId))
            return Forbid();
        var (ok, error) = await projectRepository.Delete(id);
        if (!ok)
            return BadRequest(error);
        return Ok();
    }
}
