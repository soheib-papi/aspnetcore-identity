using aspnetcore_identity.Models.Dtos;
using aspnetcore_identity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController: ControllerBase
{
    private readonly RoleManager<RoleIdentity> _roleManager;

    public RolesController(RoleManager<RoleIdentity> roleManager)
    {
        _roleManager = roleManager;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _roleManager.Roles
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s=> new RoleDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            })
            .ToListAsync(cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllAsync(long id, CancellationToken cancellationToken)
    {
        var result = await _roleManager.Roles
            .Select(s=> new RoleDto
            {
                Id = s.Id,
                Name = string.IsNullOrEmpty(s.Name) ? "" : s.Name,
                Description = s.Description
            })
            .FirstOrDefaultAsync(r=>r.Id == id, cancellationToken);
        
        if(result == null)
            return NotFound();
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(AddNewRoleDto request)
    {
        if(!ModelState.IsValid)
            return BadRequest("Input data is invalid.");
            
        RoleIdentity newRole = new RoleIdentity()
        {
            Name = request.Name,
            Description = request.Description
        };
        
        var isExist = (await _roleManager.FindByNameAsync(request.Name)) != null;
        
        if(isExist)
            return BadRequest("Role already exists.");
        
        var result = await _roleManager.CreateAsync(newRole);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        return Ok(result.Succeeded);
    }
}