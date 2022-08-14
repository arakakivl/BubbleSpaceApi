using System.Text;
using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Application.Commands.RegisterUserCommand;
using BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;
using BubbleSpaceApi.Core.Interfaces;
using BubbleSpaceApi.Core.Interfaces.Repositories;
using BubbleSpaceApi.Infra.Persistence;
using BubbleSpaceApi.Infra.Persistence.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

/// Data Access ///
builder.Services.AddDbContext<AppDbContext>(x => x.UseInMemoryDatabase("db"));

builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IProfileRepository, ProfileRepository>();
builder.Services.AddTransient<IQuestionRepository, QuestionRepository>();
builder.Services.AddTransient<IAnswerRepository, AnswerRepository>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

/// MediatR and CQRS Pattern ///
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();
builder.Services.AddMediatR(typeof(RegisterUserCommand));

/// API ///
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt => {
    opt.RequireHttpsMetadata = !(builder.Environment.IsDevelopment());
    opt.SaveToken = true;
    opt.TokenValidationParameters = new()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Auth:Secret").Value)),
        ValidateLifetime = true,
        ValidAudience = builder.Configuration.GetSection("Auth:Audience").Value,
        ValidIssuer = builder.Configuration.GetSection("Auth:Issuer").Value
    };
});

builder.Services.AddTransient<IAuth, Auth>();

builder.Services.AddControllers();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
