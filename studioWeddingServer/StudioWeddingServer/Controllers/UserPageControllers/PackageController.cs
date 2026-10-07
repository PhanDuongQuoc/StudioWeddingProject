using Microsoft.AspNetCore.Mvc;
using StudioWeddingServer.DTOs.UserPageDTOs.PackageDTOs;
using StudioWeddingServer.Services.UserPageServices.PackageServices;

namespace StudioWeddingServer.Controllers.UserPageControllers;

[ApiController]
[Route("api/[controller]")]
public class PackageController : ControllerBase
{
    private readonly IPackageService _packageService;

    public PackageController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    [HttpGet]
    public async Task<ActionResult<PackageListResponseDto>> GetPackagesAsync([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
    {
        var result = await _packageService.GetPackagesAsync(search, page, pageSize);
        return Ok(result);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetPackageDetailAsync(string slug, int orderPage = 1, int orderPageSize = 3)
    {
        var result = await _packageService.GetPackageDetailAsync(slug, orderPage, orderPageSize);
        return Ok(result);
    }
}
