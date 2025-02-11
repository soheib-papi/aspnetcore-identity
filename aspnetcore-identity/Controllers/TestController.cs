using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_identity.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TestController: ControllerBase
{
    [HttpGet("get-info")]
    public IActionResult GetInfo()
    {
        return Ok("My name is Soheib.");
    }
}