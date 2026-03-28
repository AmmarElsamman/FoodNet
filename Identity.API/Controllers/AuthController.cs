using Identity.API.Data;
using Identity.API.Dtos;
using Identity.API.Entities;
using Identity.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthContext context, ITokenService tokenService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var user = await context.Users.FirstOrDefaultAsync(e => e.Email == loginDto.Email);
            if (user == null) return NotFound();
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash)) return Unauthorized();
            return Ok(new { Id = user.Id, token = tokenService.GenerateToken(user) });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await EmailExists(registerDto.Email)) return Conflict("Email Exists");
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };


            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var userDto = new UserDto(user.Id, user.Email);
            return Created("", userDto);
        }

        private async Task<bool> EmailExists(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
