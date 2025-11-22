using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for DiscountRule entity operations
/// </summary>
public interface IDiscountRuleRepository
{
    /// <summary>
    /// Gets all active discount rules ordered by minimum quantity
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active discount rules</returns>
    Task<List<DiscountRule>> GetActiveRulesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the applicable discount rule for a given quantity
    /// </summary>
    /// <param name="quantity">The quantity to find a rule for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The applicable discount rule, or null if none applies</returns>
    Task<DiscountRule?> GetApplicableRuleAsync(int quantity, CancellationToken cancellationToken = default);
}

