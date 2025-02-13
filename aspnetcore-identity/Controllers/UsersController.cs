using aspnetcore_identity.Models.Dtos;
using aspnetcore_identity.Models.Identity;
using aspnetcore_identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_identity.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController: ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IUsersServices _usersServices;
    
    public UsersController(ILogger<UsersController> logger, UserManager<UserIdentity> userManager, IUsersServices usersServices)
    {
        _logger = logger;
        _userManager = userManager;
        _usersServices = usersServices;
    }

    [HttpGet("get-all-users")]
    public async Task<IActionResult> GetAllUsersAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _userManager.Users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPost("create-user")]
    public async Task<IResult> CreateUser(UserRegisterDto request)
    {
        if (!ModelState.IsValid)
            throw new Exception("Input data is invalid.");
        
        var result = await _usersServices.CreateUserAsync(request);
        
        return result;
    }
}