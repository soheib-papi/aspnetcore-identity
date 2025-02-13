using aspnetcore_identity.Models.Dtos;
using aspnetcore_identity.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace aspnetcore_identity.Services;

public class UsersServices: IUsersServices
{
    private readonly ILogger<UsersServices> _logger;
    private readonly UserManager<UserIdentity> _userManager;

    public UsersServices(ILogger<UsersServices> logger, UserManager<UserIdentity> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }


    public async Task<IResult> CreateUserAsync(UserRegisterDto request)
    {
        UserIdentity userIdentity = new UserIdentity()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
        };
        
        var user = await _userManager.FindByNameAsync(request.Email);
        
        if (user is not null)
            return Results.BadRequest("User already exists.");
        
        var result = await _userManager.CreateAsync(userIdentity, request.Password);

        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        return Results.Ok(result);
    }
}