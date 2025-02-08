using aspnetcore_identity.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace aspnetcore_identity.Models;

public class IdentityDatabaseContext: IdentityDbContext<UserIdentity, RoleIdentity, long>
{
    
}