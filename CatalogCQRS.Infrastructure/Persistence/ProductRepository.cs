using CatalogCQRS.Domain.Common;
using CatalogCQRS.Domain.Entities;
using Marten;

namespace CatalogCQRS.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly IDocumentStore _store;

    public ProductRepository(IDocumentStore store)
    {
        _store = store;
    }

    public async Task<Product> AddAsync(Product entity)
    {
        using var session = _store.LightweightSession();
        session.Store(entity);
        await session.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        using var session = _store.LightweightSession();
        session.Delete<Product>(id);
        await session.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var session = _store.QuerySession();
        return await session.Query<Product>().ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        using var session = _store.QuerySession();
        return await session.LoadAsync<Product>(id);
    }

    public async Task<IEnumerable<Product>> GetByCategory(string category)
    {
        using var session = _store.QuerySession();
        return await session.Query<Product>()
            .Where(p => p.Category.Contains(category))
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchProducts(string searchTerm)
    {
        using var session = _store.QuerySession();
        return await session.Query<Product>()
            .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
            .ToListAsync();
    }

    public async Task UpdateAsync(Product entity)
    {
        using var session = _store.LightweightSession();
        session.Update(entity);
        await session.SaveChangesAsync();
    }
}
