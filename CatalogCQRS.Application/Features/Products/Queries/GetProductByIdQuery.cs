using MediatR;

namespace CatalogCQRS.Application.Features.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<GetProductByIdResponse>;
