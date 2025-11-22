using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for Sale entity
/// </summary>
public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale number cannot be empty.")
            .MaximumLength(100)
            .WithMessage("Sale number cannot be longer than 100 characters.");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty()
            .WithMessage("Sale date cannot be empty.");

        RuleFor(sale => sale.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer ID must be greater than zero.");

        RuleFor(sale => sale.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name cannot be empty.")
            .MaximumLength(200)
            .WithMessage("Customer name cannot be longer than 200 characters.");

        RuleFor(sale => sale.BranchId)
            .GreaterThan(0)
            .WithMessage("Branch ID must be greater than zero.");

        RuleFor(sale => sale.BranchDescription)
            .NotEmpty()
            .WithMessage("Branch description cannot be empty.")
            .MaximumLength(200)
            .WithMessage("Branch description cannot be longer than 200 characters.");

        RuleFor(sale => sale.TotalAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total amount cannot be negative.");

        RuleFor(sale => sale.Status)
            .NotEqual(SaleStatus.Unknown)
            .WithMessage("Sale status cannot be Unknown.");

        RuleFor(sale => sale.Items)
            .NotEmpty()
            .WithMessage("Sale must have at least one item.");

        RuleForEach(sale => sale.Items)
            .SetValidator(new SaleItemValidator());

        RuleFor(sale => sale)
            .Must(sale => !sale.CancelledAt.HasValue || sale.Status == SaleStatus.Cancelled)
            .WithMessage("Cancelled date can only be set when status is Cancelled.");
    }
}

