using Microsoft.AspNetCore.Mvc;
using ProjektSklepElektronika.Models;
using ProjektSklepElektronika.Models.Auth;
using ProjektSklepElektronika.Services;
using Microsoft.AspNetCore.Authorization;
using ProjektSklepElektronika.Authentication;
using AppUser = ProjektSklepElektronika.Models.User;
using System.Security.Claims;



namespace ProjektSklepElektronika.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly JwtService _jwt;



    public AuthController(JwtService jwt)
    {
        _jwt = jwt;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterDto dto)
    {
        if (AppUser.Users.Any(u => u.Email == dto.Email))

        {
            return BadRequest("Użytkownik już istnieje");
        }

        var user = new User
        {
            Email = dto.Email,
            Password = dto.Password,
            Role = "User"
        };

        AppUser.Users.Add(user);

        return Ok("Zarejestrowano pomyślnie");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var user = AppUser.Users.SingleOrDefault(u => u.Email == dto.Email && u.Password == dto.Password);
        if (user == null)
        {
            return Unauthorized("Niepoprawne dane logowania");
        }

        var token = _jwt.GenerateToken(user.Id, user.Role);

        return Ok(new { Token = token });
    }

    [Authorize]
    [HttpGet("secret")]
    public IActionResult Secret()
    {
        return Ok("Masz ważny token – gratulacje!");
    }

    [HttpPost("forgot-password")]
    public IActionResult ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        var user = AppUser.Users.SingleOrDefault(u => u.Email == dto.Email);
        if (user == null)
        {
            return Ok();
        }

        var token = _jwt.GeneratePasswordResetToken(user.Id);

        
        return Ok(new { Token = token, Message = "Token do resetu hasła wygenerowany." });
    }

    [HttpPost("reset-password")]
    public IActionResult ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var user = AppUser.Users.SingleOrDefault(u => u.Email == dto.Email);
        if (user == null)
            return BadRequest("Nieprawidłowy email");

        var principal = _jwt.GetPrincipalFromToken(dto.Token);
        if (principal == null)
            return BadRequest("Nieprawidłowy lub wygasły token");

        var userIdFromToken = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var resetClaim = principal.FindFirst("reset_password")?.Value;

        if (userIdFromToken != user.Id.ToString() || resetClaim != "true")
            return BadRequest("Token nie pasuje do użytkownika");

        
        user.Password = dto.NewPassword; 

        return Ok("Hasło zostało zresetowane");

    }
    [Authorize]
    [HttpPut("edit-account")]
    public IActionResult EditAccount([FromBody] EditAccountDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized("Nie znaleziono Id użytkownika");

        var userId = userIdClaim.Value; 

        if (AppUser.Users.Any(u => u.Email == dto.NewEmail && u.Id != userId))
            return BadRequest("Email jest już zajęty");

        var user = AppUser.Users.SingleOrDefault(u => u.Id == userId);
        if (user == null)
            return NotFound("Użytkownik nie istnieje");

        if (!string.IsNullOrEmpty(dto.NewEmail))
            user.Email = dto.NewEmail;

        if (!string.IsNullOrEmpty(dto.NewPassword))
            user.Password = dto.NewPassword; 

        return Ok("Dane użytkownika zostały zaktualizowane");
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("change-role")]
    public IActionResult ChangeRole([FromBody] ChangeRoleDto dto)
    {
        var user = AppUser.Users.SingleOrDefault(u => u.Email == dto.Email);
        if (user == null)
            return NotFound("Użytkownik nie znaleziony");

        user.Role = dto.NewRole;
        return Ok($"Rola użytkownika {dto.Email} zmieniona na {dto.NewRole}");
    }
}
