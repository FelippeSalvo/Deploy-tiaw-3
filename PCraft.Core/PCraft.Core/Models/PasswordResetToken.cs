using System.ComponentModel.DataAnnotations.Schema;

namespace PCraft.Core.Models;

public class PasswordResetToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public Usuario Usuario { get; set; } = null!;

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public bool Used { get; set; } = false;
}