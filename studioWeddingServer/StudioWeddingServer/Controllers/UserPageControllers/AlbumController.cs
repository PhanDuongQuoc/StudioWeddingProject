using Microsoft.AspNetCore.Mvc;
using StudioWeddingServer.DTOs.UserPageDTOs.AlbumDTOs;
using StudioWeddingServer.Services.UserPageServices.AlbumServices;

namespace StudioWeddingServer.Controllers.UserPageControllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumController : ControllerBase
{
    private readonly IAlbumService _albumService;

    public AlbumController(IAlbumService albumService)
    {
        _albumService = albumService;
    }

    [HttpGet]
    public async Task<ActionResult<AlbumListResponseDto>> GetAlbums([FromQuery] string? categorySlug, [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 9)
    {
        var result = await _albumService.GetAlbumsAsync(categorySlug, search, page, pageSize);
        return Ok(result);
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<AlbumDetailDto>> Get(string slug)
    {
        var album = await _albumService.GetAlbumDetailBySlug(slug);

        if (album == null)
        {
            return NotFound(new { message = "Không tìm thấy album hoặc album đã bị ẩn." });
        }

        return Ok(album);
    }
}
