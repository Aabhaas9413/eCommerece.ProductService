using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators;

public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>
{
    public ProductAddRequestValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required.")
            .MinimumLength(2).WithMessage("Product name must be at least 2 characters long.")
            .MaximumLength(200).WithMessage("Product name must not exceed 200 characters.");

        RuleFor(x => x.Categorys)
            .IsInEnum().WithMessage("Category must be a valid value.");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Unit price must be 0 or greater.");

        RuleFor(x => x.QuantityInStock)
            .GreaterThanOrEqualTo(0)
            .When(x => x.QuantityInStock.HasValue)
            .WithMessage("Quantity in stock cannot be negative.");
    }
}
