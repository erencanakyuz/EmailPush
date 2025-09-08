using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using EmailPush.Application.Mappings;
using EmailPush.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace EmailPush.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(CampaignProfile));

        // FluentValidation
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateCampaignValidator>();

        // MediatR (if not already added)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        return services;
    }
}