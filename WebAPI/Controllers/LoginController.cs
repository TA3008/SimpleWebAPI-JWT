using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly TokenService _tokenService;

        public LoginController(DataContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterModel dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _tokenService.CreateToken(user);

            return Ok(token);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginModel dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == dto.Username);

            if (user == null)
                return Unauthorized("Invalid username");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isPasswordValid)
                return Unauthorized("Invalid password");

            var token = _tokenService.CreateToken(user);

            return Ok(token);
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult<string> GetMe()
        {
            return Ok($"Hello {User.Identity.Name}");
        }
    }
}
