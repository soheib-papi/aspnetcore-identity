using aspnetcore_identity.Models.Dtos;
using aspnetcore_identity.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace aspnetcore_identity.Services;

public class UsersServices: IUsersServices
{
    private readonly ILogger<UsersServices> _logger;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IEmailSender _emailSender;

    public UsersServices(ILogger<UsersServices> logger, 
        UserManager<UserIdentity> userManager, 
        IEmailSender emailSender)
    {
        _logger = logger;
        _userManager = userManager;
        _emailSender = emailSender;
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

        var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
        var confirmationLink = $"https://localhost:7242/api/Account/confirm-email?userId={userIdentity.Id}&token={Uri.EscapeDataString(emailConfirmToken)}";

        await _emailSender.SendEmailAsync(userIdentity.Email, "Confirm your email", $"Click <a href='{confirmationLink}'>here</a> to confirm your email.",
            CancellationToken.None);
        
        return Results.Ok(result);
    }
}