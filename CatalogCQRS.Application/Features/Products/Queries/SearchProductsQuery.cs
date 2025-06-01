using CatalogCQRS.Domain.Common;
using MediatR;

namespace CatalogCQRS.Application.Features.Products.Queries;

public record SearchProductsQuery(string SearchTerm) : IRequest<IEnumerable<GetProductByIdResponse>>;

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, IEnumerable<GetProductByIdResponse>>
{
    private readonly IProductRepository _repository;

    public SearchProductsQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetProductByIdResponse>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.SearchProducts(request.SearchTerm);
        return products.Select(GetProductByIdResponse.FromProduct);
    }
}
