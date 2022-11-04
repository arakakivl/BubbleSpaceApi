namespace BubbleSpaceApi.Api.Auth;

public class AuthSettings
{
    public string Secret { get; set; } = null!;

    public float AccessExpiration { get; set; }
    public float RefreshExpiration { get; set; }

    public bool ValidateAudience { get; set; }
    public bool ValidateIssuer { get; set; }

    public string Audience { get; set; } = null!;
    public string Issuer { get; set; } = null!;
}