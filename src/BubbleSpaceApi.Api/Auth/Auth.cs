using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BubbleSpaceApi.Api.Auth;

public class Auth : IAuth
{
    private readonly IConfiguration _config;
    public Auth(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateJwtToken(Guid profileId)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config.GetSection("Auth:Secret").Value);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Gray;

        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", profileId.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(double.Parse(_config.GetSection("Auth:AccessExpiration").Value)),
            Audience = _config.GetSection("Auth:Audience").Value,
            Issuer = _config.GetSection("Auth:Issuer").Value,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    public Guid GetProfileIdFromToken(HttpContext context)
    {
        var cookie = context.Request.Cookies.SingleOrDefault(x => x.Key == "bsacc");

        var tokenValidationParams = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Auth:Secret").Value)),
            ValidateLifetime = true,
            ValidAudience = _config.GetSection("Auth:Audience").Value,
            ValidIssuer = _config.GetSection("Auth:Issuer").Value
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(cookie.Value, tokenValidationParams, out var valid);

        if (valid is not JwtSecurityToken jwtSecurityToken || principal is null )
            throw new SecurityTokenException("Invalid jwt token.");
        else if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new Exception(SecurityAlgorithms.HmacSha256Signature);

        if (Guid.TryParse(principal.Claims.SingleOrDefault(x => x.Type == "Id")?.Value, out var profId))
            return profId;
        else
            throw new Exception("Invalid access token.");
    }
}