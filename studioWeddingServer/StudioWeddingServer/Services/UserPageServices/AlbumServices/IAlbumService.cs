using StudioWeddingServer.DTOs.UserPageDTOs.AlbumDTOs;

namespace StudioWeddingServer.Services.UserPageServices.AlbumServices;

public interface IAlbumService
{
    Task<AlbumDetailDto?> GetAlbumDetailBySlug(string slug);
}
