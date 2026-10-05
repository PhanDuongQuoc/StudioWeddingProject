using Microsoft.AspNetCore.Mvc;
using StudioWeddingServer.DTOs.UserPageDTOs.HomeDTOs;
using StudioWeddingServer.Models;
using StudioWeddingServer.Services.UserPageServices.EmailServices;
using StudioWeddingServer.Services.UserPageServices.HomeServices;
namespace StudioWeddingServer.Controllers.UserPageControllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IHomeService _service;
    private readonly ISendEmailService _sendEmailService;


    public HomeController(IHomeService service, ISendEmailService sendEmailService)
    {
        _service = service;
        _sendEmailService = sendEmailService;
    }
    [HttpGet("home-page")]
    public async Task<ActionResult<HomeDto>> Get()
    {
        var result = await _service.GetHomePageDataAsync();
        return Ok(result);
    }

    [HttpPost("send-contact-email-from-customer")]
    public async Task<ActionResult> SendContctEmailFromCustomer(ContactRequest request)
    {
        var result = await _sendEmailService.SendEmailAsync(request);
        return Ok(result);
    }
}
