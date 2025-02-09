using aspnetcore_identity.Models.Dtos;
using aspnetcore_identity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<UserIdentity> _userManager;

    public AccountController(ILogger<AccountController> logger, UserManager<UserIdentity> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet("register")]
    public IActionResult Register(UserRegisterDto request)
    {
        return Ok("AccountController is working!");
    }
}