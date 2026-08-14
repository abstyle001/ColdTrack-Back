using System.Security.Claims;
using ColdTrack_Back.Authorization;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Repositories;
using ColdTrack_Back.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ColdTrack_Back.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController(TaskRepository taskRepository) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.TaskRead)]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll(
        [FromQuery] string? assigneeId = null)
    {
        if (!User.IsInRole("Admin"))
            assigneeId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub")
                         ?? User.FindFirstValue("id");
        return Ok(await taskRepository.GetAll(assigneeId));
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
        [FromQuery] string? assigneeId = null)
    {
        // Non-admin users can only see tasks assigned to them
        if (!User.IsInRole("Admin"))
            assigneeId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub")
                         ?? User.FindFirstValue("id");
        return Ok(await taskRepository.GetPage(number, size, status, priority, assigneeId));
    }

    [HttpGet]
    [Route("count")]
    [HasPermission(Permissions.TaskRead)]
    public ActionResult<long> GetCount(
        [FromQuery] string? status = null,
        [FromQuery] string? priority = null,
        [FromQuery] string? assigneeId = null)
    {
        // Non-admin users can only see tasks assigned to them
        if (!User.IsInRole("Admin"))
            assigneeId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub")
                         ?? User.FindFirstValue("id");
        return Ok(taskRepository.GetCount(status, priority, assigneeId));
    }

    [HttpPost]
    [HasPermission(Permissions.TaskCreate)]
    public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto dto)
    {
        var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue("sub")
                        ?? User.FindFirstValue("id");
        if (string.IsNullOrEmpty(creatorId))
            return Unauthorized("无法获取用户身份");
        var task = await taskRepository.Create(dto, creatorId);
        return Ok(task);
    }

    [HttpPut]
    [Route("{id:long}")]
    [HasPermission(Permissions.TaskUpdate)]
    public async Task<ActionResult<TaskDto>> Update([FromRoute] long id, [FromBody] UpdateTaskDto dto)
    {
        var task = await taskRepository.Update(id, dto);
        if (task == null)
            return BadRequest("任务不存在");
        return Ok(task);
    }

    [HttpDelete]
    [Route("{id:long}")]
    [HasPermission(Permissions.TaskDelete)]
    public async Task<ActionResult> Delete([FromRoute] long id)
    {
        var ok = await taskRepository.Delete(id);
        if (!ok)
            return BadRequest("任务不存在");
        return Ok();
    }

    [HttpDelete]
    [Route("batch")]
    [HasPermission(Permissions.TaskDelete)]
    public async Task<ActionResult> DeleteBatch([FromBody] List<long> ids)
    {
        await taskRepository.DeleteBatch(ids);
        return Ok();
    }

    [HttpPatch]
    [Route("{id:long}/status")]
    [HasPermission(Permissions.TaskUpdate)]
    public async Task<ActionResult<TaskDto>> UpdateStatus(
        [FromRoute] long id,
        [FromBody] UpdateTaskStatusDto dto)
    {
        var assigneeId = User.IsInRole("Admin")
            ? null
            : User.FindFirstValue(ClaimTypes.NameIdentifier)
              ?? User.FindFirstValue("sub")
              ?? User.FindFirstValue("id");
        var task = await taskRepository.UpdateStatus(id, dto.Status, assigneeId);
        if (task == null)
            return BadRequest("任务不存在");
        return Ok(task);
    }

    [HttpPatch]
    [Route("batch/status")]
    [HasPermission(Permissions.TaskUpdate)]
    public async Task<ActionResult<IEnumerable<TaskDto>>> UpdateStatusBatch(
        [FromBody] BatchUpdateStatusDto dto)
    {
        var assigneeId = User.IsInRole("Admin")
            ? null
            : User.FindFirstValue(ClaimTypes.NameIdentifier)
              ?? User.FindFirstValue("sub")
              ?? User.FindFirstValue("id");
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
