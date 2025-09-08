using EmailPush.Domain.Interfaces;
using EmailPush.Domain.Services;
using EmailPush.Infrastructure.Data;
using EmailPush.Infrastructure.Repositories;
using EmailPush.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmailPush.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlite(connectionString);
        });

        // Repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ICampaignRepository, CampaignRepository>();

        // Domain Services
        services.AddScoped<ICampaignDomainService, CampaignDomainService>();

        // Infrastructure Services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IRetryService, RetryService>();
        services.AddScoped<IEmailCampaignPublisher, EmailCampaignPublisher>();

        // Cache
        services.AddMemoryCache();

        return services;
    }
}