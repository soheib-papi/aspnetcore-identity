using System.ComponentModel.DataAnnotations;

namespace aspnetcore_identity.Models.Dtos;

public class UserRegisterDto
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    [EmailAddress]
    public required string Email { get; set; }
    
    public required string PhoneNumber { get; set; }
    
    public required string Password { get; set; }
    
    [Compare(nameof(Password))]
    public required string ConfirmPassword { get; set; }
}