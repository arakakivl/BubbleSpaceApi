using System.Text;
using BubbleSpaceApi.Api;
using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Application;
using BubbleSpaceApi.Application.Commands.RegisterUserCommand;
using BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;
using BubbleSpaceApi.Core.Interfaces;
using BubbleSpaceApi.Core.Interfaces.Repositories;
using BubbleSpaceApi.Infra;
using BubbleSpaceApi.Infra.Persistence;
using BubbleSpaceApi.Infra.Persistence.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

/// Data Access ///
builder.Services.AddInfrastructureServices();

/// MediatR framework, CQRS Pattern and validators ///
builder.Services.AddApplicationServices();

/// API ///
builder.Services.AddApiServices(builder);

/// Swagger ///
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    var securityScheme = new OpenApiSecurityScheme()
    {
        BearerFormat = "JWT",
        Name = "JWT Auth",
        Description = "Put ur JWT token here for being authenticated. Only your token! Not \"Bearer\"!",
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Reference = new OpenApiReference()
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    x.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    x.AddSecurityRequirement( new OpenApiSecurityRequirement()
    {
        { securityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
