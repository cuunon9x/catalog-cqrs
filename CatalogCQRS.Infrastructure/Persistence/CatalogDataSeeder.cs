using CatalogCQRS.Domain.Common;
using CatalogCQRS.Domain.Entities;
using Marten;

namespace CatalogCQRS.Infrastructure.Persistence;

public class CatalogDataSeeder
{
    public static async Task SeedDataAsync(IDocumentStore store)
    {
        using var session = store.LightweightSession();

        if (!await session.Query<Product>().AnyAsync())
        {
            // Store products one by one
            session.Store(new Product(
                "Sample Product 1",
                new List<string> { "Electronics", "Gadgets" },
                "This is a sample product description",
                "sample1.jpg",
                new Money(299.99m)));
                
            session.Store(new Product(
                "Sample Product 2",
                new List<string> { "Home", "Kitchen" },
                "Another sample product description",
                "sample2.jpg",
                new Money(49.99m)));
                
            session.Store(new Product(
                "Sample Product 3",
                new List<string> { "Electronics", "Computers" },
                "Yet another sample product",
                "sample3.jpg",
                new Money(999.99m)));

            await session.SaveChangesAsync();
        }
    }
}
