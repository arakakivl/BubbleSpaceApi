using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BubbleSpaceApi.Api.Auth;

public static class Auth
{
    private static readonly IConfiguration _config;
    static Auth()
    {
        _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                    .Build();
    }

    public static string GenerateJwtToken(Guid profileId)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config.GetSection("Auth:Secret").Value);

        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", profileId.ToString())
            }),
            Expires = DateTime.Now.AddHours(double.Parse(_config.GetSection("Auth:AccessExpiration").Value)),
            Audience = _config.GetSection("Auth:Audience").Value,
            Issuer = _config.GetSection("Auth:Issuer").Value,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }

    public static string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    public static Guid GetProfileIdFromToken(this HttpContext context)
    {
        var cookie = context.Request.Cookies.SingleOrDefault(x => x.Key == "bsacc");

        var tokenValidationParams = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("Auth:Secret").Value)),
            ValidateLifetime = true
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(cookie.Value, tokenValidationParams, out var valid);

        if (valid is not JwtSecurityToken jwtSecurityToken 
            || principal is null
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid jwt token.");
        
        if (Guid.TryParse(principal.Claims.SingleOrDefault(x => x.Type == "Id")?.Value, out var profId))
            return profId;
        else
            throw new Exception("Invalid access token.");
    }
}