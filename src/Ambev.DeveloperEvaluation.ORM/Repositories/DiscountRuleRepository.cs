using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IDiscountRuleRepository using Entity Framework Core
/// </summary>
public class DiscountRuleRepository : IDiscountRuleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of DiscountRuleRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public DiscountRuleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all active discount rules ordered by minimum quantity (descending to get best discount first)
    /// </summary>
    public async Task<List<DiscountRule>> GetActiveRulesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DiscountRules
            .Where(r => r.IsActive)
            .OrderByDescending(r => r.MinQuantity) // Order by highest minimum to get best discount first
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the applicable discount rule for a given quantity
    /// Returns the rule with the highest minimum quantity that applies to the given quantity
    /// </summary>
    public async Task<DiscountRule?> GetApplicableRuleAsync(int quantity, CancellationToken cancellationToken = default)
    {
        return await _context.DiscountRules
            .Where(r => r.IsActive && quantity >= r.MinQuantity && quantity <= r.MaxQuantity)
            .OrderByDescending(r => r.MinQuantity) // Get the best discount (highest minimum)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

