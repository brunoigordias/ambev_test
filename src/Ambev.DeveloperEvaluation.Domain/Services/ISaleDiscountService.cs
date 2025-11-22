namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Service for calculating discounts based on business rules
/// </summary>
public interface ISaleDiscountService
{
    /// <summary>
    /// Calculates the discount percentage based on quantity
    /// Business rules:
    /// - Below 4 items: no discount (0%)
    /// - 4-9 items: 10% discount
    /// - 10-20 items: 20% discount
    /// - Above 20 items: not allowed (throws exception)
    /// </summary>
    /// <param name="quantity">The quantity of items</param>
    /// <returns>The discount percentage (0-100)</returns>
    /// <exception cref="DomainException">Thrown when quantity exceeds 20</exception>
    decimal CalculateDiscountPercentage(int quantity);

    /// <summary>
    /// Validates if the quantity is allowed according to business rules
    /// </summary>
    /// <param name="quantity">The quantity to validate</param>
    /// <returns>True if quantity is valid, false otherwise</returns>
    bool IsQuantityValid(int quantity);

    /// <summary>
    /// Calculates the discount amount based on quantity and unit price
    /// </summary>
    /// <param name="quantity">The quantity of items</param>
    /// <param name="unitPrice">The unit price</param>
    /// <returns>The discount amount</returns>
    decimal CalculateDiscountAmount(int quantity, decimal unitPrice);

    /// <summary>
    /// Calculates the total amount after discount
    /// </summary>
    /// <param name="quantity">The quantity of items</param>
    /// <param name="unitPrice">The unit price</param>
    /// <returns>The total amount after discount</returns>
    decimal CalculateTotalAmount(int quantity, decimal unitPrice);
}

