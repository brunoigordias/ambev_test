using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for DiscountRule entity
/// </summary>
public class DiscountRuleValidator : AbstractValidator<DiscountRule>
{
    public DiscountRuleValidator()
    {
        RuleFor(rule => rule.MinQuantity)
            .GreaterThan(0)
            .WithMessage("Minimum quantity must be greater than zero.");

        RuleFor(rule => rule.MaxQuantity)
            .GreaterThan(0)
            .WithMessage("Maximum quantity must be greater than zero.");

        RuleFor(rule => rule)
            .Must(rule => rule.MaxQuantity >= rule.MinQuantity)
            .WithMessage("Maximum quantity must be greater than or equal to minimum quantity.");

        RuleFor(rule => rule.DiscountPercentage)
            .InclusiveBetween(0, 100)
            .WithMessage("Discount percentage must be between 0 and 100.");

        RuleFor(rule => rule.MinQuantity)
            .LessThanOrEqualTo(SaleBusinessRules.MAX_QUANTITY_ALLOWED)
            .WithMessage($"Minimum quantity cannot exceed {SaleBusinessRules.MAX_QUANTITY_ALLOWED}.");

        RuleFor(rule => rule.MaxQuantity)
            .LessThanOrEqualTo(SaleBusinessRules.MAX_QUANTITY_ALLOWED)
            .WithMessage($"Maximum quantity cannot exceed {SaleBusinessRules.MAX_QUANTITY_ALLOWED}.");
    }
}

