using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

/// <summary>
/// Specification to check if a sale item is eligible for discount
/// Business rule: Only items with quantity >= 4 are eligible for discount
/// </summary>
public class SaleDiscountEligibilitySpecification : ISpecification<SaleItem>
{
    /// <summary>
    /// Checks if the sale item is eligible for discount
    /// </summary>
    /// <param name="entity">The sale item to check</param>
    /// <returns>True if item is eligible for discount, false otherwise</returns>
    public bool IsSatisfiedBy(SaleItem entity)
    {
        return entity.Quantity >= SaleBusinessRules.MIN_QUANTITY_FOR_DISCOUNT;
    }
}

