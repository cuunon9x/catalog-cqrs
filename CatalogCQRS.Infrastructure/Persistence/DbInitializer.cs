using Marten;
using Microsoft.Extensions.Hosting;

namespace CatalogCQRS.Infrastructure.Persistence;

public class DbInitializer : IHostedService
{
    private readonly IDocumentStore _store;

    public DbInitializer(IDocumentStore store)
    {
        _store = store;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await CatalogDataSeeder.SeedDataAsync(_store);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
