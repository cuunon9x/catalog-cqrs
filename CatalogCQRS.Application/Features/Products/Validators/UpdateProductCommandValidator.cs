using FluentValidation;

namespace CatalogCQRS.Application.Features.Products.Commands;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Category)
            .NotEmpty()
            .Must(x => x.Any())
            .WithMessage("At least one category must be specified");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.ImageFile)
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(x => x.Price)
            .NotNull()
            .Must(x => x.Amount > 0)
            .WithMessage("Price must be greater than 0");
    }
}
