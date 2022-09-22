using System.Text;
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

var builder = WebApplication.CreateBuilder(args);

/// Data Access ///
builder.Services.AddInfrastructureServices();

/// MediatR framework, CQRS Pattern and validators ///
builder.Services.AddApplicationServices();

/// API ///
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt => {
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

builder.Services.AddTransient<IAuth, Auth>();
builder.Services.AddControllers(opt => opt.SuppressAsyncSuffixInActionNames = false);

/// Swagger ///

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
