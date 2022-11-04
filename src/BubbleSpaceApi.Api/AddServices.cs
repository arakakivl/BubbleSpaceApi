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
                ValidateAudience = Convert.ToBoolean(builder.Configuration.GetSection("AuthSettings:ValidateAudience").Value),
                ValidateIssuer = Convert.ToBoolean(builder.Configuration.GetSection("AuthSettings:ValidateIssuer").Value),
                ValidAudience = builder.Configuration.GetSection("AuthSettings:Audience").Value,
                ValidIssuer = builder.Configuration.GetSection("AuthSettings:Issuer").Value,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AuthSettings:Secret").Value)),
                ValidateLifetime = true
            };
        });

        collection.AddTransient<IAuth, Auth.Auth>(provider => 
        {
            var settings = builder.Configuration.GetSection(nameof(AuthSettings)).Get<AuthSettings>();
            return new Auth.Auth(settings);
        });
        
        collection.AddControllers(opt => opt.SuppressAsyncSuffixInActionNames = false);
        return collection;
    }
}