using Microsoft.AspNetCore.Mvc;
using StudioWeddingServer.DTOs.UserPageDTOs.HomeDTOs;
using StudioWeddingServer.Models;
using StudioWeddingServer.Services.UserPageServices.HomeServices;
namespace StudioWeddingServer.Controllers.UserPageControllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IHomeService _service;
    public HomeController(IHomeService service)
    {
        _service = service;
    }
    [HttpGet("home-page")]
    public async Task<ActionResult<HomeDto>> Get()
    {
        var result = await _service.GetHomePageDataAsync();
        return Ok(result);
    }
}
