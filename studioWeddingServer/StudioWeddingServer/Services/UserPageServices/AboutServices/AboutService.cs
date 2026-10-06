using Microsoft.EntityFrameworkCore;
using StudioWeddingServer.DTOs.UserPageDTOs.AboutDTOs;
using StudioWeddingServer.Models;

namespace StudioWeddingServer.Services.UserPageServices.AboutServices;

public class AboutService : IAboutService
{
    private readonly StudioWeddingDbContext _context;

    public AboutService(StudioWeddingDbContext context)
    {
        _context = context;
    }
    public async Task<AboutResponse> GetAboutAsync(int? aboutId = null)
    {
        var query = _context.AboutUs.Where(a => a.IsActive == true);
        if (aboutId.HasValue && aboutId.Value > 0)
        {
            query = query.Where(a => a.AboutId == aboutId.Value);
        }

        var about = await query
            .Select(a => new AboutResponse
            {
                AboutId = a.AboutId,
                HeroTitle = a.HeroTitle,
                HeroSubtitle = a.HeroSubtitle,
                HeroImageUrl = a.HeroImageUrl,
                StoryTitle = a.StoryTitle,
                StoryContent = a.StoryContent,
                StoryImageUrl = a.StoryImageUrl,
                FounderName = a.FounderName,
                FounderQuote = a.FounderQuote,
                YearsExperience = a.YearsExperience ?? 0,
                HappyCouples = a.HappyCouples ?? 0,
                SatisfactionRate = a.SatisfactionRate,
                Timelines = a.AboutTimelines.OrderBy(t => t.DisplayOrder).Select(t => new AboutTimelineDto
                {
                    TimelineId = t.TimelineId,
                    Year = t.Year,
                    Title = t.Title,
                    Description = t.Description,
                    DisplayOrder = t.DisplayOrder ?? 0
                }).ToList(),
                Success = true,
                Message = "Lấy dữ liệu thành công"
            }).FirstOrDefaultAsync();

        if (about == null)
        {
            return new AboutResponse
            {
                Success = false,
                Message = "Không tìm thấy dữ liệu Giới thiệu"
            };
        }

        return about;
    }

}
