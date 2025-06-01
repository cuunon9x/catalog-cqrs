using MediatR;

namespace CatalogCQRS.Application.Features.Products.Commands;

public record CreateProductCommand : IRequest<Guid>
{
    public string Name { get; init; } = default!;
    public List<string> Category { get; init; } = new();
    public string Description { get; init; } = default!;
    public string ImageFile { get; init; } = default!;
    public decimal Price { get; init; }
}
