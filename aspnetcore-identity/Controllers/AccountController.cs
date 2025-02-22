using aspnetcore_identity.Models.Dtos;
using aspnetcore_identity.Models.Identity;
using aspnetcore_identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly SignInManager<UserIdentity> _signInManager;
    private readonly IUsersServices _usersServices;
    private readonly IEmailSender _emailSender;

    public AccountController(ILogger<AccountController> logger, 
        UserManager<UserIdentity> userManager, 
        SignInManager<UserIdentity> signInManager, 
        IUsersServices usersServices, 
        IEmailSender emailSender)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _usersServices = usersServices;
        _emailSender = emailSender;
    }

    [HttpPost("register")]
    public async Task<IResult> Register(UserRegisterDto request)
    {
        if (!ModelState.IsValid)
            throw new Exception("Input data is invalid.");

        var result = await _usersServices.CreateUserAsync(request);
        
        return result;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            throw new Exception("Input data is invalid.");
        
        var user = await _userManager.FindByNameAsync(request.Email);
        
        if (user == null)
            return NotFound("User not found.");

        await _signInManager.SignOutAsync();
        
        var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);

        if (!result.Succeeded)
        {
            return BadRequest("Sign in failed.");
        }
        
        return Ok(result.Succeeded);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
    
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null) return BadRequest("Invalid user");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return Ok("Email confirmed successfully!");
        }

        return BadRequest("Email confirmation failed");
    }

    [Authorize]
    [HttpPost("forget-password-token")]
    public async Task<IActionResult> GetForgetPasswordTokenAsync(ForgetPasswordDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Input data is invalid.");
        
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null) 
            return BadRequest("Invalid user");
        
        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        if(!isEmailConfirmed)
            return BadRequest("Email is not confirmed.");
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        var confirmationLink = $"https://localhost:7242/api/Account/reset-password?userId={user.Id}&token={Uri.EscapeDataString(token)}";

        await _emailSender.SendEmailAsync(user.Email!, "Your password reset link", $"Your UserId={user.Id}\nToken={token}",
            CancellationToken.None);
        
        return Ok();
    }

    [Authorize]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto request)
    {
        if(!ModelState.IsValid)
            return BadRequest("Input data is invalid.");
        
        UserIdentity? user = await _userManager.FindByIdAsync(request.UserId.ToString());
        
        if(user == null)
            return BadRequest("Invalid user");
        
        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        
        if(!result.Succeeded)
            return BadRequest("Operation failed.");
        
        return Ok();
    }

    [Authorize]
    [HttpPost("set-mobile-number")]
    public async Task<IActionResult> SetMobileNumberAsync(SetMobileNumberRequest request)
    {
        if(!ModelState.IsValid)
            return BadRequest("Input data is invalid.");
        
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        
        if(user == null)
            return BadRequest("Invalid user");
        
        IdentityResult? result = await _userManager.SetPhoneNumberAsync(user, request.MobileNumber);
        
        string code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, request.MobileNumber);
        
        //TODO: Implement Sms service and send code via sms to user's phone number
        
        return Ok(code);
    }

    [Authorize]
    [HttpPost("verify-mobile-number")]
    public async Task<IActionResult> VerifyMobileNumberAsync(VerifyMobileNumberRequest request)
    {
        if(!ModelState.IsValid)
            return BadRequest("Input data is invalid.");
        
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        
        if(user == null)
            return BadRequest("Invalid user");
        
        bool isVerified = await _userManager.VerifyChangePhoneNumberTokenAsync(user, request.Code, request.MobileNumber);
        
        if(!isVerified)
            return BadRequest("Operation failed.");
        
        IdentityResult? result = await _userManager.SetPhoneNumberAsync(user, request.MobileNumber);
        user.PhoneNumberConfirmed = isVerified;
        await _userManager.UpdateAsync(user);
        
        return Ok(result.Succeeded);
    }
}