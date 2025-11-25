using FluentValidation;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

namespace Ambev.DeveloperEvaluation.WebApi.Validation;

/// <summary>
/// Validator for UpdateCartRequest
/// </summary>
public class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
{
    public UpdateCartRequestValidator()
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

