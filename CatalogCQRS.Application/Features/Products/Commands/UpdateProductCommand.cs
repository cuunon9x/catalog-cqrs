using CatalogCQRS.Domain.Common;
using MediatR;

namespace CatalogCQRS.Application.Features.Products.Commands;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    Money Price) : IRequest<Unit>;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductRepository _repository;

    public UpdateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id) ?? 
            throw new KeyNotFoundException($"Product with ID {request.Id} not found");

        product.UpdateProduct(
            request.Name,
            request.Category,
            request.Description,
            request.ImageFile,
            request.Price);

        await _repository.UpdateAsync(product);
        return Unit.Value;
    }
}
