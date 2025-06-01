using CatalogCQRS.Domain.Common;
using MediatR;

namespace CatalogCQRS.Application.Features.Products.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<Unit>;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _repository;

    public DeleteProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id) ?? 
            throw new KeyNotFoundException($"Product with ID {request.Id} not found");
            
        await _repository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
