using System.ComponentModel.DataAnnotations;

namespace aspnetcore_identity.Models.Dtos;

public class VerifyMobileNumberRequest
{
    [Required]
    [RegularExpression(@"^09[0-9]{9}$", ErrorMessage = "Please enter a valid mobile number")]
    public string MobileNumber { get; set; }

    [Required]
    public string Code { get; set; }
}