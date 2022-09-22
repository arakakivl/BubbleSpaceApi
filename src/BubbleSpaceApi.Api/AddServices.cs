using System.Text;
using BubbleSpaceApi.Api.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BubbleSpaceApi.Api;


public static class AddServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection collection, WebApplicationBuilder builder)
    {
        collection.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            opt.RequireHttpsMetadata = !(builder.Environment.IsDevelopment());
            opt.SaveToken = true;
            opt.TokenValidationParameters = new()
            {
                ValidateAudience = int.Parse(builder.Configuration.GetSection("Auth:ValidateAudience").Value) == 1,
                ValidateIssuer = int.Parse(builder.Configuration.GetSection("Auth:ValidateIssuer").Value) == 1,
                ValidAudience = builder.Configuration.GetSection("Auth:ValidAudience").Value,
                ValidIssuer = builder.Configuration.GetSection("Auth:ValidIssuer").Value,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Auth:Secret").Value)),
                ValidateLifetime = true
            };
        });

        collection.AddTransient<IAuth, Auth.Auth>();
        collection.AddControllers(opt => opt.SuppressAsyncSuffixInActionNames = false);

        return collection;
    }
}