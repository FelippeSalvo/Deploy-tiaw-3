using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;
using PCraft.Core.DTOs;
using PCraft.Core.Models;
using PCraft.Core.Services;
using System.Security.Cryptography;
using System.Text;

namespace PCraft.Core.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _config;

    public AuthController(
        AppDbContext context,
        IEmailService emailService,
        IConfiguration config)
    {
        _context = context;
        _emailService = emailService;
        _config = config;
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            return Ok(new { message = "Se o e-mail existir, enviaremos um link de recuperação." });
        }

        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var resetToken = new PasswordResetToken
        {
            UserId = user.Id,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            Used = false
        };

        _context.PasswordResetTokens.Add(resetToken);
        await _context.SaveChangesAsync();

        var frontendUrl = _config["Frontend:ResetPasswordUrl"];
        var resetLink = $"{frontendUrl}?token={Uri.EscapeDataString(token)}";

        await _emailService.SendPasswordResetEmailAsync(user.Email, resetLink);

        return Ok(new { message = "Se o e-mail existir, enviaremos um link de recuperação." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        var resetToken = await _context.PasswordResetTokens
            .FirstOrDefaultAsync(t =>
                t.Token == dto.Token &&
                t.Used == false &&
                t.ExpiresAt > DateTime.UtcNow);

        if (resetToken == null)
        {
            return BadRequest(new { message = "Token inválido ou expirado." });
        }

        var user = await _context.Users.FindAsync(resetToken.UserId);

        if (user == null)
        {
            return BadRequest(new { message = "Usuário não encontrado." });
        }

        user.PasswordHash = HashPassword(dto.NewPassword);

        resetToken.Used = true;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Senha redefinida com sucesso." });
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}