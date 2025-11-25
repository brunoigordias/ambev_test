using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

/// <summary>
/// Validator for CreateCartCommand
/// </summary>
public class CreateCartValidator : AbstractValidator<CreateCartCommand>
{
    public CreateCartValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than zero");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Cart date is required");

        RuleFor(x => x.Products)
            .NotNull().WithMessage("Products list cannot be null");

        RuleForEach(x => x.Products)
            .SetValidator(new CartProductDtoValidator());
    }
}

/// <summary>
/// Validator for CartProductDto
/// </summary>
public class CartProductDtoValidator : AbstractValidator<CartProductDto>
{
    public CartProductDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than zero");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero");
    }
}

