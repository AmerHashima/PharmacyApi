using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Auth;

public class LoginDto
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}