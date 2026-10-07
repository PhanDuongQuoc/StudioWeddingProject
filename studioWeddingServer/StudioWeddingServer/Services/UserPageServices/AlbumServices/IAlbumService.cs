using StudioWeddingServer.DTOs.UserPageDTOs.AlbumDTOs;

namespace StudioWeddingServer.Services.UserPageServices.AlbumServices;

public interface IAlbumService
{
    Task<AlbumListResponseDto> GetAlbumsAsync(string? categorySlug, string? search, int page = 1, int pageSize = 9);
    Task<AlbumDetailDto?> GetAlbumDetailBySlug(string slug);
}
