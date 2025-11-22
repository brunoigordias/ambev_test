using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Implementation of discount calculation service based on business rules from database
/// </summary>
public class SaleDiscountService : ISaleDiscountService
{
    private readonly IDiscountRuleRepository _discountRuleRepository;

    /// <summary>
    /// Initializes a new instance of SaleDiscountService
    /// </summary>
    /// <param name="discountRuleRepository">The discount rule repository</param>
    public SaleDiscountService(IDiscountRuleRepository discountRuleRepository)
    {
        _discountRuleRepository = discountRuleRepository;
    }

    /// <summary>
    /// Calculates the discount percentage based on quantity using rules from database
    /// </summary>
    public async Task<decimal> CalculateDiscountPercentageAsync(int quantity, CancellationToken cancellationToken = default)
    {
        if (!IsQuantityValid(quantity))
        {
            throw new DomainException($"It's not possible to sell above {SaleBusinessRules.MAX_QUANTITY_ALLOWED} identical items.");
        }

        // Get applicable discount rule from database
        var applicableRule = await _discountRuleRepository.GetApplicableRuleAsync(quantity, cancellationToken);

        if (applicableRule != null)
        {
            return applicableRule.DiscountPercentage;
        }

        // No discount rule applies
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
    public async Task<decimal> CalculateDiscountAmountAsync(int quantity, decimal unitPrice, CancellationToken cancellationToken = default)
    {
        var discountPercentage = await CalculateDiscountPercentageAsync(quantity, cancellationToken);
        var subtotal = quantity * unitPrice;
        return subtotal * (discountPercentage / 100);
    }

    /// <summary>
    /// Calculates the total amount after discount
    /// </summary>
    public async Task<decimal> CalculateTotalAmountAsync(int quantity, decimal unitPrice, CancellationToken cancellationToken = default)
    {
        var subtotal = quantity * unitPrice;
        var discountAmount = await CalculateDiscountAmountAsync(quantity, unitPrice, cancellationToken);
        return subtotal - discountAmount;
    }
}

