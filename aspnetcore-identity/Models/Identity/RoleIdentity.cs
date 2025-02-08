using Microsoft.AspNetCore.Identity;

namespace aspnetcore_identity.Models.Identity;

public class RoleIdentity: IdentityRole<long>
{
    public string Description { get; set; }
}