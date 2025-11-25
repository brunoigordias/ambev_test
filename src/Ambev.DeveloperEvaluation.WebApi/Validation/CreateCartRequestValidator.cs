using FluentValidation;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

namespace Ambev.DeveloperEvaluation.WebApi.Validation;

/// <summary>
/// Validator for CreateCartRequest
/// </summary>
public class CreateCartRequestValidator : AbstractValidator<CreateCartRequest>
{
    public CreateCartRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than zero");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Cart date is required");

        RuleFor(x => x.Products)
            .NotNull().WithMessage("Products list cannot be null");

        RuleForEach(x => x.Products)
            .SetValidator(new CartProductRequestValidator());
    }
}

/// <summary>
/// Validator for CartProductRequest
/// </summary>
public class CartProductRequestValidator : AbstractValidator<CartProductRequest>
{
    public CartProductRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than zero");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero");
    }
}

