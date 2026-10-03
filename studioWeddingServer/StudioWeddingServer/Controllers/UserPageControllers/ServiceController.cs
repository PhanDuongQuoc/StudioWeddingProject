using Microsoft.AspNetCore.Mvc;
using StudioWeddingServer.Services.UserPageServices.ServiceServices;

namespace StudioWeddingServer.Controllers.UserPageControllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServiceController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetServiceDetail(string slug, int otherPage = 1, int otherPageSize = 3)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return BadRequest(new { Success = false, Message = "Slug không được để trống" });
        }
        var result = await _serviceService.GetServiceDetailAsync(slug, otherPage, otherPageSize);
        return Ok(result);
    }
}
