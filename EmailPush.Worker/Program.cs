using EmailPush.Worker;
using EmailPush.Infrastructure.Data;
using EmailPush.Domain.Interfaces;
using EmailPush.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository
builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EmailConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

// Ensure database is created
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

var host = builder.Build();
host.Run();
