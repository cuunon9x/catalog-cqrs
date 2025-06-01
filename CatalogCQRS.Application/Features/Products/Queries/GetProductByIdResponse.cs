using CatalogCQRS.Domain.Common;
using CatalogCQRS.Domain.Entities;

namespace CatalogCQRS.Application.Features.Products.Queries;

public record GetProductByIdResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public List<string> Category { get; init; } = new();
    public string Description { get; init; } = default!;
    public string ImageFile { get; init; } = default!;
    public Money Price { get; init; } = default!;

    private GetProductByIdResponse() { }

    public GetProductByIdResponse(
        Guid id,
        string name,
        List<string> category,
        string description,
        string imageFile,
        Money price)
    {
        Id = id;
        Name = name;
        Category = category;
        Description = description;
        ImageFile = imageFile;
        Price = price;
    }

    public static GetProductByIdResponse FromProduct(Product product) =>
        new(
            product.Id,
            product.Name,
            product.Category,
            product.Description,
            product.ImageFile,
            product.Price
        );
}
