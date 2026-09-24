using Microsoft.AspNetCore.Mvc;

namespace StudioWeddingServer.Controllers.UserControllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello world");
    }
}
