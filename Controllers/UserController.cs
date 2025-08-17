using ColdTrack_Back.Dtos;
using ColdTrack_Back.Models;
using ColdTrack_Back.Repositories;
using ColdTrack_Back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColdTrack_Back.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(UserRepository userRepository) : ControllerBase
{
    [HttpGet]
    [Route("{id}")]
    [Authorize(Roles = RoleType.User)]
    public ActionResult<AppUser> GetUser([FromRoute] string id)
    {
        var userDto = userRepository.GetUserInfo(id);
        if (userDto == null)
        {
            return NotFound();
        }
        return Ok(userDto);
    }

    [HttpPut]
    [Route("{id}")]
    [RequestSizeLimit(10_000_000)]
    [Authorize(Roles = RoleType.User)]
    public async Task<ActionResult<UserDto>> UpdateUser([FromRoute] string id, [FromForm] UpdateUserDto dto)
    {
        var userDto = await userRepository.UpdateUser(id, dto);
        if (userDto == null)
        {
            return BadRequest("用户不存在");
        }
        return Ok(userDto);
    }

    [HttpGet]
    [Route("list")]
    [Authorize(Roles = RoleType.User)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUser()
    {
        var userList = await userRepository.GetAllUser();
        return Ok(userList);
    }

    [HttpGet]
    [Route("count")]
    [Authorize(Roles = RoleType.User)]
    public ActionResult<long> GetUserCount()
    {
        return Ok(userRepository.GetUserCount());
    }

    [HttpGet]
    [Route("page")]
    [Authorize(Roles = RoleType.User)]
    public async Task<IEnumerable<UserDto>> GetUserPage([FromQuery] int number, [FromQuery] int size)
    {
        return await userRepository.GetUserPage(number, size);
    }

    [HttpDelete]
    [Route("batch")]
    [Authorize(Roles = RoleType.Admin)]
    public async Task<IActionResult> DeleteUserBatch([FromBody] List<string> ids)
    {
        if (ids.Count == 0)
        {
            return BadRequest("请选入要删除的用户列表");
        }
        await userRepository.DeleteUserBatch(ids);
        return Ok();
    }
}