namespace BubbleSpaceApi.Api.Auth;

public interface IAuth
{
    public Guid GetProfileIdFromToken(string token);
    public string GenerateToken(Dictionary<string, string> claims, bool isRefresh = false);

    public bool IsAuthenticated(string token);
}