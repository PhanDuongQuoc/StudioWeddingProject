using Microsoft.EntityFrameworkCore;
using StudioWeddingServer.DTOs.UserPageDTOs.ServiceDTOs;
using StudioWeddingServer.Models;

namespace StudioWeddingServer.Services.UserPageServices.ServiceServices;

public class ServiceService : IServiceService
{
    private readonly StudioWeddingDbContext _context;

    public ServiceService(StudioWeddingDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceListResponseDto> GetServicesAsync(string? search, int page = 1, int pageSize = 12)
    {
        if (page < 1) page = 1;
        if (pageSize <= 0) pageSize = 12;

        var query = _context.Services
            .AsNoTracking()
            .Where(s => s.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(s => s.Name.ToLower().Contains(term) || (s.Description != null && s.Description.ToLower().Contains(term)));
        }

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = await query
            .OrderBy(s => s.ServiceId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new ServiceItemDto
            {
                ServiceId = s.ServiceId,
                Name = s.Name,
                Slug = s.Slug,
                Description = s.Description,
                Price = s.Price,
                DurationMinutes = s.DurationMinutes,
                ImageUrl = s.ImageUrl,
                CountBooking = s.BookingItems.Count(),
                CreatedAt = s.CreatedAt
            })
            .ToListAsync();

        return new ServiceListResponseDto
        {
            Items = items,
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            Success = true,
            Message = "Thành công"
        };
    }

    public async Task<ServiceDetailRespone> GetServiceDetailAsync(string slug, int otherPage, int otherPageSize)
    {
        // check valid of slug 
        if (string.IsNullOrEmpty(slug))
        {
            return new ServiceDetailRespone
            {
                Success = false,
                Message = "Slug không được để trống"
            };
        }
        if (otherPage <= 0)
        {
            otherPage = 1;
        }
        if (otherPageSize <= 0)
        {
            otherPageSize = 3;
        }

        var isSerive = await _context.Services.AsNoTracking()
            .Include(s => s.BookingItems)
            .FirstOrDefaultAsync(s => s.Slug == slug && s.IsActive);

        if (isSerive == null)
        {
            return new ServiceDetailRespone
            {
                Success = false,
                Message = "Không tìm thấy dịch vụ"
            };
        }

        return new ServiceDetailRespone
        {
            ServiceId = isSerive.ServiceId,
            Name = isSerive.Name,
            Slug = isSerive.Slug,
            Description = isSerive.Description,
            Price = isSerive.Price,
            DurationMinutes = isSerive.DurationMinutes,
            ImageUrl = isSerive.ImageUrl,
            CountBooking = isSerive.BookingItems.Count(),
            CreatedAt = isSerive.CreatedAt,
            UpdatedAt = isSerive.UpdatedAt,
            OtherServices = await _context.Services
                .Include(s => s.BookingItems)
                .Where(s => s.Slug != isSerive.Slug && s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .Skip((otherPage - 1) * otherPageSize)
                .Take(otherPageSize)
                .Select(s => new RelatedServiceRespone
                {
                    ServiceId = s.ServiceId,
                    Name = s.Name,
                    Slug = s.Slug,
                    Price = s.Price,
                    ImageUrl = s.ImageUrl
                }).ToListAsync(),
            OtherPage = otherPage,
            OtherPageSize = otherPageSize,
            OtherTotal = await _context.Services
                .Where(s => s.Slug != isSerive.Slug && s.IsActive)
                .CountAsync(),
            Success = true,
            Message = "Thành công"
        };
    }
}
