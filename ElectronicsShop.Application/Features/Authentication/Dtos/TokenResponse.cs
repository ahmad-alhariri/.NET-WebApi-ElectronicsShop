namespace ElectronicsShop.Application.Features.Authentication.Dtos;

public class TokenResponse
{
    public string AccessToken { get; set; }
    public RefreshTokenDto RefreshTokenDto { get; set; }
}

public class RefreshTokenDto
{
    public string UserName { get; set; }
    public string TokenString { get; set; }
    public DateTimeOffset ExpireAt { get; set; }
}