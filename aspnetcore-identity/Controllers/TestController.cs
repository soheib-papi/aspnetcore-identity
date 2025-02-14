using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_identity.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TestController: ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("get-info1")]
    public IActionResult GetInfo1()
    {
        return Ok("My name is Soheib {get-info1}.");
    }
    
    [Authorize(Roles = "Editor")]
    [HttpGet("get-info2")]
    public IActionResult GetInfo2()
    {
        return Ok("My name is Soheib {get-info2}.");
    }
}