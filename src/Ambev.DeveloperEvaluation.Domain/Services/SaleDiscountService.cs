using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Implementation of discount calculation service based on business rules
/// </summary>
public class SaleDiscountService : ISaleDiscountService
{

    /// <summary>
    /// Calculates the discount percentage based on quantity
    /// Business rules:
    /// - Below 4 items: no discount (0%)
    /// - 4-9 items: 10% discount
    /// - 10-20 items: 20% discount
    /// - Above 20 items: not allowed (throws exception)
    /// </summary>
    public decimal CalculateDiscountPercentage(int quantity)
    {
        if (!IsQuantityValid(quantity))
        {
            throw new DomainException($"It's not possible to sell above {SaleBusinessRules.MAX_QUANTITY_ALLOWED} identical items.");
        }

        // Business rule: Below 4 items cannot have discount
        if (quantity < SaleBusinessRules.MIN_QUANTITY_FOR_DISCOUNT)
        {
            return 0;
        }

        // Business rule: 10-20 items have 20% discount
        if (quantity >= SaleBusinessRules.MIN_QUANTITY_FOR_HIGH_DISCOUNT && quantity <= SaleBusinessRules.MAX_QUANTITY_ALLOWED)
        {
            return SaleBusinessRules.HIGH_DISCOUNT_PERCENTAGE;
        }

        // Business rule: 4+ items have 10% discount
        if (quantity >= SaleBusinessRules.MIN_QUANTITY_FOR_DISCOUNT)
        {
            return SaleBusinessRules.LOW_DISCOUNT_PERCENTAGE;
        }

        return 0;
    }

    /// <summary>
    /// Validates if the quantity is allowed according to business rules
    /// </summary>
    public bool IsQuantityValid(int quantity)
    {
        return quantity >= SaleBusinessRules.MIN_QUANTITY && quantity <= SaleBusinessRules.MAX_QUANTITY_ALLOWED;
    }

    /// <summary>
    /// Calculates the discount amount based on quantity and unit price
    /// </summary>
    public decimal CalculateDiscountAmount(int quantity, decimal unitPrice)
    {
        var discountPercentage = CalculateDiscountPercentage(quantity);
        var subtotal = quantity * unitPrice;
        return subtotal * (discountPercentage / 100);
    }

    /// <summary>
    /// Calculates the total amount after discount
    /// </summary>
    public decimal CalculateTotalAmount(int quantity, decimal unitPrice)
    {
        var subtotal = quantity * unitPrice;
        var discountAmount = CalculateDiscountAmount(quantity, unitPrice);
        return subtotal - discountAmount;
    }
}

