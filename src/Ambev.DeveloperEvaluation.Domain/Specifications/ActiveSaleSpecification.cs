using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

/// <summary>
/// Specification to check if a sale is active (not cancelled)
/// </summary>
public class ActiveSaleSpecification : ISpecification<Sale>
{
    /// <summary>
    /// Checks if the sale is active
    /// </summary>
    /// <param name="entity">The sale to check</param>
    /// <returns>True if sale is active, false otherwise</returns>
    public bool IsSatisfiedBy(Sale entity)
    {
        return entity.Status == SaleStatus.Active;
    }
}

