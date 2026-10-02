
using Microsoft.AspNetCore.Mvc;
using StudioWeddingServer.DTOs.UserPageDTOs.AuthDTOs;
using StudioWeddingServer.Services.AuthServices.LoginServices;
using StudioWeddingServer.Services.AuthServices.RegisterServices;
using StudioWeddingServer.Services.ForgotServices;
namespace StudioWeddingServer.Controllers.AuthControllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILoginService _loginservice;
    private readonly IRegisterService _registerservice;
    private readonly IForgotService _forgotservice;
    public AuthController(ILoginService loginservice, IRegisterService registerservice, IForgotService forgotservice)
    {
        _loginservice = loginservice;
        _registerservice = registerservice;
        _forgotservice = forgotservice;
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

    [HttpPost("send-forgot-otp")]
    public async Task<IActionResult> SendForgotOtp(SendForgotOtpRequest request)
    {
        var result = await _forgotservice.SendForgotOtpAsync(request);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPost("reset-password-with-otp")]
    public async Task<IActionResult> ResetPasswordWithOtp(ResetPasswordWithOtpRequest request)
    {
        var result = await _forgotservice.ResetPasswordWithOtpAsync(request);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

}






