using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.JSInterop.Infrastructure;
using Projetize.Api.Data;
using Projetize.Api.DTOs.Login;
using Projetize.Api.DTOs.User;
using Projetize.Api.Models;
using Projetize.Api.Models.Login;
using Projetize.Api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Projetize.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;
        public UsersController(AppDBContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO dto)
        {
            var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailExists)
                return Conflict("Email já está em uso.");

            var userNameExists = await _context.Users.AnyAsync(u => u.UserName == dto.UserName);
            if (userNameExists)
                return Conflict("Nome de usuário já está em uso.");

            Guid confirmationToken = Guid.NewGuid();

            User user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                UserName = dto.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreateAt = DateTime.Now,
                IsActive = true,
                EmailConfirmationToken = confirmationToken.ToString().Substring(0, 6),
                EmailConfirmationTokenExpiresAt = DateTime.Now.AddMinutes(15)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userResponseDto = new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                LastLogin = user.LastLogin
            };

            await _emailService.SendConfirmationEmailAsync(user.Email, user.Name, user.EmailConfirmationToken);

            return CreatedAtAction(nameof(Register), new { id = user.Id }, userResponseDto);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDTO dto)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == dto.RefreshToken);

            if (refreshToken == null || refreshToken.RevokedAt != null || refreshToken.ExpirationDate < DateTime.UtcNow)
                return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == refreshToken.UserId);
            if (user == null)
                return Unauthorized("Usuário não encontrado.");

            var revokedToken = new RevokedToken
            {
                Token = refreshToken.Token,
                RevokedAt = DateTime.UtcNow,
                ExpiryDate = refreshToken.ExpirationDate,
            };

            _context.RevokedTokens.Add(revokedToken);
            await _context.SaveChangesAsync();

            refreshToken = _jwtService.GenerateRefreshToken(user.Id);
            _context.RefreshTokens.Add(refreshToken);

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken.Token,
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            var login = dto.Login;

            var user = login.Contains("@") ? await _context.Users.FirstOrDefaultAsync(u => u.Email == login) : await _context.Users.FirstOrDefaultAsync(u => u.UserName == login);

            if (user == null)
                return Unauthorized("Usuário não encontrado.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return Unauthorized("Credenciais inválidas.");

            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
            _context.RefreshTokens.Add(refreshToken);

            var userLoginResponseDTO = new UserLoginResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                LastLogin = user.LastLogin
            };

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                User = userLoginResponseDTO
            });
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var id = Guid.Parse(userId);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound("Usuário não encontrado.");

            var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id);
            if (emailExists)
                return Conflict("Email já está em uso.");

            var userNameExists = await _context.Users.AnyAsync(u => u.UserName == dto.UserName && u.Id != id);
            if (userNameExists)
                return Conflict("Nome de usuário já está em uso.");

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.UserName = dto.UserName;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var userUpdateResponse = new UserUpdateResponseDTO
            {
                Id = id,
                Name = user.UserName,
                Email = user.Email,
                UserName = user.UserName
            };

            return Ok(userUpdateResponse);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token não fornecido");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var exp = jwtToken.ValidTo;
            var jti = jwtToken.Id;

            var revokedToken = new RevokedToken
            {
                Token = !string.IsNullOrEmpty(jti) ? jti : token,
                RevokedAt = DateTime.UtcNow,
                ExpiryDate = exp
            };

            _context.RevokedTokens.Add(revokedToken);
            await _context.SaveChangesAsync();

            return Ok("Logout efetuado com sucesso.");
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmationDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return Unauthorized("Email não econtrado");

            if (user.EmailConfirmationToken != dto.Token)
                return Unauthorized("Código inválido");

            if (user.EmailConfirmationTokenExpiresAt < DateTime.UtcNow)
                return Unauthorized("Código expirado.");

            user.EmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.EmailConfirmationTokenExpiresAt = DateTime.MinValue;

            await _context.SaveChangesAsync();

            return Ok("Email confirmado com sucesso.");
        }

        [HttpGet("test-email")]
        public async Task<IActionResult> TestEmail([FromServices] IEmailService emailService)
        {
            await emailService.SendConfirmationEmailAsync(
                "vi.i.konflanz@gmail.com",
                "Marcus Konflanz",
                "123456"
            );

            return Ok("E-mail enviado com sucesso.");
        }
    }
}