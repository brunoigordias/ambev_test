namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Service for calculating discounts based on business rules from database
/// </summary>
public interface ISaleDiscountService
{
    /// <summary>
    /// Calculates the discount percentage based on quantity using rules from database
    /// </summary>
    /// <param name="quantity">The quantity of items</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The discount percentage (0-100)</returns>
    /// <exception cref="DomainException">Thrown when quantity exceeds maximum allowed</exception>
    Task<decimal> CalculateDiscountPercentageAsync(int quantity, CancellationToken cancellationToken = default);

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
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The discount amount</returns>
    Task<decimal> CalculateDiscountAmountAsync(int quantity, decimal unitPrice, CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculates the total amount after discount
    /// </summary>
    /// <param name="quantity">The quantity of items</param>
    /// <param name="unitPrice">The unit price</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The total amount after discount</returns>
    Task<decimal> CalculateTotalAmountAsync(int quantity, decimal unitPrice, CancellationToken cancellationToken = default);
}

