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

        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[] { new Claim("Id", profileId.ToString()) }),
            Audience = _config.GetSection("Auth:Audience").Value,            
            Issuer = _config.GetSection("Auth:Issuer").Value,
            Expires = DateTime.UtcNow.AddHours(double.Parse(_config.GetSection("Auth:AccessExpiration").Value)),
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
            ValidAudience = _config.GetSection("Auth:ValidAudience").Value,
            ValidIssuer = _config.GetSection("Auth:ValidAudience").Value,
            ValidateAudience = int.Parse(_config.GetSection("Auth:ValidateAudience").Value) == 1,
            ValidateIssuer = int.Parse(_config.GetSection("Auth:ValidateIssuer").Value) == 1,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("Auth:Secret").Value))
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