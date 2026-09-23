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
public class TaskController(TaskRepository taskRepository, ProjectRepository projectRepository, IPermissionCacheService cacheService) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.TaskRead)]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll(
        [FromQuery] long? projectId = null,
        [FromQuery] string? assigneeId = null)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub")
                     ?? User.FindFirstValue("id") ?? "";
        var isAdmin = User.IsInRole("Admin");
        return Ok(await taskRepository.GetAll(userId, isAdmin, projectId, assigneeId));
    }

    [HttpGet]
    [Route("{id:long}")]
    [HasPermission(Permissions.TaskRead)]
    public async Task<ActionResult<TaskDto>> GetById([FromRoute] long id)
    {
        var task = await taskRepository.GetById(id);
        if (task == null)
            return NotFound("任务不存在");
        return Ok(task);
    }

    [HttpGet]
    [Route("page")]
    [HasPermission(Permissions.TaskRead)]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetPage(
        [FromQuery] int number,
        [FromQuery] int size,
        [FromQuery] string? status = null,
        [FromQuery] string? priority = null,
        [FromQuery] string? assigneeId = null,
        [FromQuery] long? tagId = null,
        [FromQuery] long? projectId = null)
    {
        // 可见性（本人任务 + 本人负责项目的任务）在仓库层按 userId/isAdmin 叠加
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub")
                     ?? User.FindFirstValue("id") ?? "";
        var isAdmin = User.IsInRole("Admin");
        return Ok(await taskRepository.GetPage(number, size, status, priority, assigneeId, tagId, projectId, userId, isAdmin));
    }

    [HttpGet]
    [Route("count")]
    [HasPermission(Permissions.TaskRead)]
    public ActionResult<long> GetCount(
        [FromQuery] string? status = null,
        [FromQuery] string? priority = null,
        [FromQuery] string? assigneeId = null,
        [FromQuery] long? tagId = null,
        [FromQuery] long? projectId = null)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub")
                     ?? User.FindFirstValue("id") ?? "";
        var isAdmin = User.IsInRole("Admin");
        return Ok(taskRepository.GetCount(status, priority, assigneeId, tagId, projectId, userId, isAdmin));
    }

    // 手动鉴权：拥有 task.create 权限，或目标项目的负责人
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto dto)
    {
        var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue("sub")
                        ?? User.FindFirstValue("id");
        if (string.IsNullOrEmpty(creatorId))
            return Unauthorized("无法获取用户身份");
        var perms = await cacheService.GetPermissionsAsync(creatorId);
        if (!perms.Contains(Permissions.TaskCreate) && !await projectRepository.IsManager(dto.ProjectId, creatorId))
            return Forbid();
        try
        {
            return Ok(await taskRepository.Create(dto, creatorId));
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    // 手动鉴权：拥有 task.update 权限，或该任务所属项目的负责人
    [HttpPut]
    [Route("{id:long}")]
    [Authorize]
    public async Task<ActionResult<TaskDto>> Update([FromRoute] long id, [FromBody] UpdateTaskDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub")
                     ?? User.FindFirstValue("id");
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("无法获取用户身份");
        var perms = await cacheService.GetPermissionsAsync(userId);
        if (!perms.Contains(Permissions.TaskUpdate) && !await projectRepository.IsManagerOfTask(id, userId))
            return Forbid();
        try
        {
            var task = await taskRepository.Update(id, dto);
            if (task == null)
                return BadRequest("任务不存在");
            return Ok(task);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    // 手动鉴权：拥有 task.delete 权限，或该任务所属项目的负责人
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
        if (!perms.Contains(Permissions.TaskDelete) && !await projectRepository.IsManagerOfTask(id, userId))
            return Forbid();
        var ok = await taskRepository.Delete(id);
        if (!ok)
            return BadRequest("任务不存在");
        return Ok();
    }

    // 手动鉴权：拥有 task.delete 权限，或全部任务所属项目的负责人
    [HttpDelete]
    [Route("batch")]
    [Authorize]
    public async Task<ActionResult> DeleteBatch([FromBody] List<long> ids)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub")
                     ?? User.FindFirstValue("id");
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("无法获取用户身份");
        var perms = await cacheService.GetPermissionsAsync(userId);
        if (!perms.Contains(Permissions.TaskDelete))
        {
            foreach (var id in ids)
            {
                if (!await projectRepository.IsManagerOfTask(id, userId))
                    return Forbid();
            }
        }
        await taskRepository.DeleteBatch(ids);
        return Ok();
    }

    // 手动鉴权：拥有 task.update 权限或项目负责人可改任意任务状态；普通用户只能改分配给自己的任务
    [HttpPatch]
    [Route("{id:long}/status")]
    [Authorize]
    public async Task<ActionResult<TaskDto>> UpdateStatus(
        [FromRoute] long id,
        [FromBody] UpdateTaskStatusDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub")
                     ?? User.FindFirstValue("id");
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("无法获取用户身份");
        var perms = await cacheService.GetPermissionsAsync(userId);
        string? assigneeId = userId;
        if (perms.Contains(Permissions.TaskUpdate) || await projectRepository.IsManagerOfTask(id, userId))
            assigneeId = null;
        var task = await taskRepository.UpdateStatus(id, dto.Status, assigneeId);
        if (task == null)
            return BadRequest("任务不存在");
        return Ok(task);
    }

    // 手动鉴权：拥有 task.update 权限或全部任务所属项目的负责人可批量改任意任务状态；普通用户只能改分配给自己的任务
    [HttpPatch]
    [Route("batch/status")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TaskDto>>> UpdateStatusBatch(
        [FromBody] BatchUpdateStatusDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub")
                     ?? User.FindFirstValue("id");
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("无法获取用户身份");
        var perms = await cacheService.GetPermissionsAsync(userId);
        string? assigneeId = userId;
        if (perms.Contains(Permissions.TaskUpdate))
        {
            assigneeId = null;
        }
        else if (dto.Ids.Count > 0)
        {
            var managerOfAll = true;
            foreach (var taskId in dto.Ids)
            {
                if (!await projectRepository.IsManagerOfTask(taskId, userId))
                {
                    managerOfAll = false;
                    break;
                }
            }
            if (managerOfAll)
                assigneeId = null;
        }
        var tasks = await taskRepository.UpdateStatusBatch(
            dto.Ids,
            dto.Status,
            assigneeId);
        return Ok(tasks);
    }

    [HttpGet]
    [Route("{id:long}/comments")]
    [HasPermission(Permissions.TaskRead)]
    public async Task<ActionResult<IEnumerable<TaskCommentDto>>> GetComments([FromRoute] long id)
    {
        if (!await taskRepository.TaskExists(id))
            return NotFound("任务不存在");
        return Ok(await taskRepository.GetComments(id));
    }

    [HttpPost]
    [Route("{id:long}/comments")]
    [HasPermission(Permissions.TaskComment)]
    public async Task<ActionResult<TaskCommentDto>> AddComment(
        [FromRoute] long id,
        [FromBody] CreateTaskCommentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (string.IsNullOrWhiteSpace(dto.Content))
            return BadRequest("评论内容不能为空");

        var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue("sub")
                       ?? User.FindFirstValue("id");
        if (string.IsNullOrEmpty(authorId))
            return Unauthorized("无法获取用户身份");

        var comment = await taskRepository.AddComment(id, dto.Content.Trim(), authorId);
        if (comment == null)
            return NotFound("任务不存在");
        return Ok(comment);
    }

    [HttpGet]
    [Route("stats")]
    [HasPermission(Permissions.TaskRead)]
    public async Task<ActionResult<TaskStatsDto>> GetStats()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub")
                     ?? User.FindFirstValue("id") ?? "";
        var isAdmin = User.IsInRole("Admin");
        return Ok(await taskRepository.GetStats(userId, isAdmin));
    }

}
