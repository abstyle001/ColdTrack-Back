using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ColdTrack_Back.Datas;
using ColdTrack_Back.Dtos;
using ColdTrack_Back.Models;
using ColdTrack_Back.Services;
using ColdTrack_Back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColdTrack_Back.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(
    ColdTrackDbContext db,
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    TokenService tokenService,
    IConfiguration config,
    IWebHostEnvironment env) : ControllerBase
{
    [HttpPost("register")]
    [RequestSizeLimit(10_000_000)]
    // [Authorize(Roles = RoleType.Admin)]
    public async Task<IActionResult> Register([FromForm] RegisterUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user != null)
            {
                return BadRequest("用户已存在");
            }

            var appUser = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                NickName = dto.NickName ?? string.Empty,
                City = dto.City ?? string.Empty,
                PhoneNumber = dto.Phone ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };
            var createUser = await userManager.CreateAsync(appUser, dto.Password);
            if (createUser.Succeeded)
            {
                var roleResult = await userManager.AddToRoleAsync(appUser, "User");
                if (!roleResult.Succeeded) return StatusCode(500, roleResult.Errors);
                var roles = await userManager.GetRolesAsync(appUser);
                var avatar = await AvatarUtil.UploadAvatar(appUser.Id, dto.File, config, env);
                appUser.Avatar = avatar;
                db.Users.Update(appUser);
                await db.SaveChangesAsync();
                return Ok(tokenService.CreateToken(appUser, roles));
            }

            return StatusCode(500, createUser.Errors);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login([FromBody] LoginUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null)
            {
                return BadRequest("用户不存在");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
            {
                return BadRequest("密码错误");
            }

            var roles = await userManager.GetRolesAsync(user);
            return Ok(tokenService.CreateToken(user, roles));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("me")]
    [Authorize(Roles = RoleType.User)]
    public ActionResult<TokenClaim> GetProfile()
    {
        var id = User.FindFirstValue("id");
        var email = User.FindFirstValue(JwtRegisteredClaimNames.Email);
        var userName = User.FindFirstValue(JwtRegisteredClaimNames.Name);
        var role = User.FindFirstValue(ClaimTypes.Role);
        return Ok(new TokenClaim
        {
            Id = id ?? string.Empty,
            Email = email ?? string.Empty,
            UserName = userName ?? string.Empty,
            Role = role ?? string.Empty
        });
    }
}