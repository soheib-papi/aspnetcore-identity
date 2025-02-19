using System.ComponentModel.DataAnnotations;

namespace aspnetcore_identity.Models.Dtos;

public class ForgetPasswordDto
{
    [EmailAddress]
    public required string Email { get; set; }
}