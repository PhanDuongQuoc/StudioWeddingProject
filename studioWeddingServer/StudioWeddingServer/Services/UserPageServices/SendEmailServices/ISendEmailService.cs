namespace StudioWeddingServer.Services.UserPageServices.EmailServices;

public interface ISendEmailService
{
    Task<ContactRespone> SendEmailAsync(ContactRequest request);
}
