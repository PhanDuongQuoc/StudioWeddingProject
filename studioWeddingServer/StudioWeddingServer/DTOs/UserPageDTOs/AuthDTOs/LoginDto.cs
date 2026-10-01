namespace StudioWeddingServer.DTOs.UserPageDTOs.AuthDTOs;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;

    public string TokenType { get; set; } = "Bearer";

    public UserProfileDto User { get; set; } = new();

    public List<string> Roles { get; set; } = [];

    public List<string> Permissions { get; set; } = [];
}

public class UserProfileDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}