using Microsoft.AspNetCore.Mvc;
using StudioWeddingServer.DTOs.UserPageDTOs.AboutDTOs;
using StudioWeddingServer.Services.UserPageServices.AboutServices;

namespace StudioWeddingServer.Controllers.UserPageControllers;

[ApiController]
[Route("api/[controller]")]
public class AboutController : ControllerBase
{
    private readonly IAboutService _aboutService;
    public AboutController(IAboutService aboutService)
    {
        _aboutService = aboutService;
    }
    [HttpGet]
    [HttpGet("{aboutId:int}")]
    public async Task<ActionResult<AboutResponse>> GetAboutAsync(int? aboutId = null)
    {
        var about = await _aboutService.GetAboutAsync(aboutId);
        if (about == null || !about.Success)
        {
            return NotFound(about ?? new AboutResponse { Success = false, Message = "Không tìm thấy dữ liệu Giới thiệu" });
        }
        return Ok(about);
    }
}
