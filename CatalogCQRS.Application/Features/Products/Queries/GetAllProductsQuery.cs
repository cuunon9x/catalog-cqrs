using CatalogCQRS.Domain.Common;
using CatalogCQRS.Domain.Entities;
using MediatR;

namespace CatalogCQRS.Application.Features.Products.Queries;

public record GetAllProductsQuery : IRequest<IEnumerable<GetProductByIdResponse>>;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<GetProductByIdResponse>>
{
    private readonly IProductRepository _repository;

    public GetAllProductsQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetProductByIdResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync();        
        return products.Select(GetProductByIdResponse.FromProduct);
    }
}
