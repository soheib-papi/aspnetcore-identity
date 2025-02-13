using aspnetcore_identity.Models.Dtos;
using aspnetcore_identity.Models.Identity;
using aspnetcore_identity.Services;
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
    private readonly UsersServices _usersServices;

    public AccountController(ILogger<AccountController> logger, 
        UserManager<UserIdentity> userManager, 
        SignInManager<UserIdentity> signInManager, 
        UsersServices usersServices)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _usersServices = usersServices;
    }

    [HttpPost("register")]
    public async Task<IResult> Register(UserRegisterDto request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            throw new Exception("Input data is invalid.");

        var result = await _usersServices.CreateUserAsync(request, cancellationToken);
        
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
}