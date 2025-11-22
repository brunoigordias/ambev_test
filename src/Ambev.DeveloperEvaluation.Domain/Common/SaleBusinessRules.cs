namespace Ambev.DeveloperEvaluation.Domain.Common;

/// <summary>
/// Constants for sale business rules
/// </summary>
public static class SaleBusinessRules
{
    /// <summary>
    /// Minimum quantity required to be eligible for discount
    /// </summary>
    public const int MIN_QUANTITY_FOR_DISCOUNT = 4;

    /// <summary>
    /// Minimum quantity for high discount tier (20%)
    /// </summary>
    public const int MIN_QUANTITY_FOR_HIGH_DISCOUNT = 10;

    /// <summary>
    /// Maximum quantity allowed per product
    /// </summary>
    public const int MAX_QUANTITY_ALLOWED = 20;

    /// <summary>
    /// Discount percentage for low tier (4-9 items)
    /// </summary>
    public const decimal LOW_DISCOUNT_PERCENTAGE = 10m;

    /// <summary>
    /// Discount percentage for high tier (10-20 items)
    /// </summary>
    public const decimal HIGH_DISCOUNT_PERCENTAGE = 20m;

    /// <summary>
    /// Minimum quantity allowed (must be at least 1)
    /// </summary>
    public const int MIN_QUANTITY = 1;
}

