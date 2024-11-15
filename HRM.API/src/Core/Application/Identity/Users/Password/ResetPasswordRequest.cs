using System.ComponentModel.DataAnnotations;

namespace MasterPOS.API.Application.Identity.Users.Password;

public class ResetPasswordRequest
{
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; } = default!;
    [Required]
    public string Token { get; set; } = default!;
}