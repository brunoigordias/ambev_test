using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

/// <summary>
/// Specification to validate if a sale item quantity meets business rules
/// </summary>
public class SaleQuantitySpecification : ISpecification<SaleItem>
{
    /// <summary>
    /// Checks if the sale item quantity is valid according to business rules
    /// </summary>
    /// <param name="entity">The sale item to validate</param>
    /// <returns>True if quantity is valid, false otherwise</returns>
    public bool IsSatisfiedBy(SaleItem entity)
    {
        return entity.Quantity >= SaleBusinessRules.MIN_QUANTITY && 
               entity.Quantity <= SaleBusinessRules.MAX_QUANTITY_ALLOWED;
    }
}

