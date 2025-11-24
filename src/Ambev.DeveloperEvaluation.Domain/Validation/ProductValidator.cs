using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for Product entity
/// </summary>
public class ProductValidator : AbstractValidator<Product>
{
    /// <summary>
    /// Initializes validation rules for Product
    /// </summary>
    public ProductValidator()
    {
        RuleFor(product => product.Title)
            .NotEmpty().WithMessage("Product title is required")
            .MaximumLength(200).WithMessage("Product title must not exceed 200 characters");

        RuleFor(product => product.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero");

        RuleFor(product => product.Description)
            .NotEmpty().WithMessage("Product description is required")
            .MaximumLength(1000).WithMessage("Product description must not exceed 1000 characters");

        RuleFor(product => product.Category)
            .NotEmpty().WithMessage("Product category is required")
            .MaximumLength(100).WithMessage("Product category must not exceed 100 characters");

        RuleFor(product => product.Image)
            .MaximumLength(500).WithMessage("Product image URL must not exceed 500 characters");

        RuleFor(product => product.Rating.Rate)
            .GreaterThanOrEqualTo(0).WithMessage("Rating rate must be greater than or equal to zero")
            .LessThanOrEqualTo(5).WithMessage("Rating rate must be less than or equal to 5");

        RuleFor(product => product.Rating.Count)
            .GreaterThanOrEqualTo(0).WithMessage("Rating count must be greater than or equal to zero");
    }
}


