using CatalogCQRS.Domain.Entities;

namespace CatalogCQRS.Domain.Common;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategory(string category);
    Task<IEnumerable<Product>> SearchProducts(string searchTerm);
}
