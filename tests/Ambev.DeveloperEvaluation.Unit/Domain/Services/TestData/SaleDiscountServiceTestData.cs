using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Services.TestData;

/// <summary>
/// Provides methods for generating test data for SaleDiscountService tests.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleDiscountServiceTestData
{
    /// <summary>
    /// Generates a valid discount rule for low tier (4-9 items, 10% discount)
    /// </summary>
    public static DiscountRule GenerateLowTierDiscountRule()
    {
        return new DiscountRule(
            SaleBusinessRules.MIN_QUANTITY_FOR_DISCOUNT,
            9,
            SaleBusinessRules.LOW_DISCOUNT_PERCENTAGE);
    }

    /// <summary>
    /// Generates a valid discount rule for high tier (10-20 items, 20% discount)
    /// </summary>
    public static DiscountRule GenerateHighTierDiscountRule()
    {
        return new DiscountRule(
            SaleBusinessRules.MIN_QUANTITY_FOR_HIGH_DISCOUNT,
            SaleBusinessRules.MAX_QUANTITY_ALLOWED,
            SaleBusinessRules.HIGH_DISCOUNT_PERCENTAGE);
    }

    /// <summary>
    /// Generates a custom discount rule
    /// </summary>
    public static DiscountRule GenerateCustomDiscountRule(int minQuantity, int maxQuantity, decimal discountPercentage)
    {
        return new DiscountRule(minQuantity, maxQuantity, discountPercentage);
    }

    /// <summary>
    /// Generates an inactive discount rule
    /// </summary>
    public static DiscountRule GenerateInactiveDiscountRule()
    {
        var rule = new DiscountRule(4, 9, 10m);
        rule.Deactivate();
        return rule;
    }

    /// <summary>
    /// Generates a valid quantity within the allowed range
    /// </summary>
    public static int GenerateValidQuantity()
    {
        return new Faker().Random.Int(
            SaleBusinessRules.MIN_QUANTITY,
            SaleBusinessRules.MAX_QUANTITY_ALLOWED);
    }

    /// <summary>
    /// Generates a quantity below the minimum allowed
    /// </summary>
    public static int GenerateQuantityBelowMinimum()
    {
        return new Faker().Random.Int(0, SaleBusinessRules.MIN_QUANTITY - 1);
    }

    /// <summary>
    /// Generates a quantity above the maximum allowed
    /// </summary>
    public static int GenerateQuantityAboveMaximum()
    {
        return new Faker().Random.Int(SaleBusinessRules.MAX_QUANTITY_ALLOWED + 1, 100);
    }

    /// <summary>
    /// Generates a valid unit price
    /// </summary>
    public static decimal GenerateValidUnitPrice()
    {
        return new Faker().Random.Decimal(1m, 1000m);
    }
}

