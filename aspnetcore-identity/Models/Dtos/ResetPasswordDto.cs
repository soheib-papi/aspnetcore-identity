using System.ComponentModel.DataAnnotations;

namespace aspnetcore_identity.Models.Dtos;

public class ResetPasswordDto
{
    [Required]
    public long UserId { get; set; }
    
    [Required]
    public string Token { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}