
using Microsoft.AspNetCore.Mvc;
using StudioWeddingServer.DTOs.UserPageDTOs.AuthDTOs;
using StudioWeddingServer.Services.AuthServices.LoginServices;
using StudioWeddingServer.Services.AuthServices.RegisterServices;
namespace StudioWeddingServer.Controllers.AuthControllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILoginService _loginservice;
    private readonly IRegisterService _registerservice;
    public AuthController(ILoginService loginservice, IRegisterService registerservice)
    {
        _loginservice = loginservice;
        _registerservice = registerservice;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _loginservice.LoginAsync(request);
        if (!result.Success)
        {
            return Unauthorized(result);
        }
        return Ok(result);
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _registerservice.RegisterAsync(request);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}






