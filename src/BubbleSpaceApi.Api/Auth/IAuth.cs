namespace BubbleSpaceApi.Api.Auth;

public interface IAuth
{
    public string GenerateJwtToken(Guid profileId);
    public string GenerateRefreshToken();

    public Guid GetProfileIdFromToken(HttpContext context);
}