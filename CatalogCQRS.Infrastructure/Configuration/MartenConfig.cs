using CatalogCQRS.Domain.Common;
using CatalogCQRS.Infrastructure.Persistence;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;

namespace CatalogCQRS.Infrastructure.Configuration;

public static class MartenConfig
{
    public static IServiceCollection AddMartenDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var store = DocumentStore.For(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            options.Connection(connectionString);
            options.DatabaseSchemaName = "public";
            options.AutoCreateSchemaObjects = AutoCreate.All;
              options.Schema.For<Domain.Entities.Product>()
                .Index(x => x.Name)
                .Index(x => x.Category)
                .UseOptimisticConcurrency(true);
        });

        services.AddSingleton<IDocumentStore>(store);
        services.AddScoped<IDocumentSession>(sp => sp.GetRequiredService<IDocumentStore>().LightweightSession());
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddHostedService<DbInitializer>();

        return services;
    }
}
