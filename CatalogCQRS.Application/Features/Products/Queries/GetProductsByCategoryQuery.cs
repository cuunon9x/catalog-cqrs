using CatalogCQRS.Domain.Common;
using MediatR;

namespace CatalogCQRS.Application.Features.Products.Queries;

public record GetProductsByCategoryQuery(string Category) : IRequest<IEnumerable<GetProductByIdResponse>>;

public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, IEnumerable<GetProductByIdResponse>>
{
    private readonly IProductRepository _repository;

    public GetProductsByCategoryQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetProductByIdResponse>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetByCategory(request.Category);
        return products.Select(GetProductByIdResponse.FromProduct);
    }
}
