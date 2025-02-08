using Microsoft.AspNetCore.Identity;

namespace aspnetcore_identity.Models.Identity;

public class UserIdentity: IdentityUser<long>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}