using CatalogCQRS.Domain.Common;
using CatalogCQRS.Infrastructure.Persistence;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;

namespace CatalogCQRS.Infrastructure.Configuration;

public static class MartenConfig
{    public static IServiceCollection AddMartenDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddMarten(options =>
        {
            options.Connection(connectionString);
            options.DatabaseSchemaName = "public";
            options.AutoCreateSchemaObjects = AutoCreate.All;

            // Configure document mappings
            options.Schema.For<Domain.Entities.Product>()
                .Index(x => x.Name)
                .Index(x => x.Category)
                .UseOptimisticConcurrency(true);
        });

        // Register and execute initial data seeding
        services.AddHostedService<DbInitializer>();

        return services;
    }
}
