using FluentValidation;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Validation;

/// <summary>
/// Validator for CreateProductRequest
/// </summary>
public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Product title is required")
            .MaximumLength(200).WithMessage("Product title must not exceed 200 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description is required")
            .MaximumLength(1000).WithMessage("Product description must not exceed 1000 characters");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Product category is required")
            .MaximumLength(100).WithMessage("Product category must not exceed 100 characters");

        RuleFor(x => x.Image)
            .MaximumLength(500).WithMessage("Product image URL must not exceed 500 characters");

        RuleFor(x => x.Rating.Rate)
            .GreaterThanOrEqualTo(0).WithMessage("Rating rate must be greater than or equal to zero")
            .LessThanOrEqualTo(5).WithMessage("Rating rate must be less than or equal to 5");

        RuleFor(x => x.Rating.Count)
            .GreaterThanOrEqualTo(0).WithMessage("Rating count must be greater than or equal to zero");
    }
}


