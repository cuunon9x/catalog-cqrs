using CatalogCQRS.Domain.Common;
using Mapster;
using MediatR;

namespace CatalogCQRS.Application.Features.Products.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {request.Id} not found");
        }

        return product.Adapt<GetProductByIdResponse>();
    }
}
