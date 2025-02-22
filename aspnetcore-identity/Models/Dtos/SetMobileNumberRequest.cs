using System.ComponentModel.DataAnnotations;

namespace aspnetcore_identity.Models.Dtos;

public class SetMobileNumberRequest
{
    [Required]
    [RegularExpression(@"^09[0-9]{9}$", ErrorMessage = "Please enter a valid mobile number")]
    public string MobileNumber { get; set; }
}