using Microsoft.AspNetCore.Mvc;
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


    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetPackageDetailAsync(string slug, int orderPage = 1, int orderPageSize = 3)
    {
        var result = await _packageService.GetPackageDetailAsync(slug, orderPage, orderPageSize);
        return Ok(result);
    }
}
