using BubbleSpaceApi.Application.Commands.RegisterUserCommand;
using BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BubbleSpaceApi.Application;

public static class AddServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection collection)
    {
        collection.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();
        collection.AddMediatR(typeof(RegisterUserCommand));
    
        // In order to use automatic validations, we can't use async rules for our validators!
        // collection.AddFluentValidationAutoValidation();

        return collection;
    }
}