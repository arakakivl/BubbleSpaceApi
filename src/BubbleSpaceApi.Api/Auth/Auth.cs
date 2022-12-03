using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BubbleSpaceApi.Api.Auth;

public class Auth : IAuth
{
    private readonly AuthSettings _settings;
    public Auth(AuthSettings settings)
    {
        _settings = settings;
    }

    public string GenerateToken(Dictionary<string, string> dictionaryClaims, bool isRefresh = false)
    {
        var claims = new List<Claim>();
        foreach(KeyValuePair<string, string> s in dictionaryClaims)
            claims.Add(new Claim(s.Key, s.Value));

        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.Secret);

        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Audience = _settings.ValidateAudience ? _settings.Audience : "",
            Issuer = _settings.ValidateIssuer ? _settings.Issuer : "",
            Expires = DateTime.UtcNow.AddHours(isRefresh ? _settings.RefreshExpiration : _settings.AccessExpiration),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }
    public Guid GetProfileIdFromToken(string token)
    {
        var claims = GetClaims(token);
        return Guid.Parse(claims.FirstOrDefault(x => x.Type == "ProfileId")!.Value);
    }

    public bool IsAuthenticated(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;
            
        var jwtSecurityToken = new JwtSecurityToken(token);
        if (jwtSecurityToken is null || (jwtSecurityToken.ValidFrom > DateTimeOffset.UtcNow) || (jwtSecurityToken.ValidTo < DateTimeOffset.UtcNow))
            return false;
        
        return true;
    }

    private IEnumerable<Claim> GetClaims(string token)
    {
        var validationParams = new TokenValidationParameters()
        {
            ValidateAudience = _settings.ValidateAudience,
            ValidateIssuer = _settings.ValidateIssuer,
            ValidAudience = _settings.ValidateAudience ? _settings.Audience : "",
            ValidIssuer = _settings.ValidateIssuer ? _settings.Issuer : "",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Secret))
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token, validationParams, out var valid);

        if (valid is not JwtSecurityToken jwtSecurityToken || principal is null)
            throw new SecurityTokenException("Invalid JWT token.");
        
        return principal.Claims;
    }
}