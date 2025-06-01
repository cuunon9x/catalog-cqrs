using CatalogCQRS.Domain.Common;
using CatalogCQRS.Domain.Entities;
using MediatR;

namespace CatalogCQRS.Application.Features.Products.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(
            request.Name,
            request.Category,
            request.Description,
            request.ImageFile,
            new Money(request.Price));

        await _productRepository.AddAsync(product);

        return product.Id;
    }
}
