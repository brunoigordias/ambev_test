using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a discount rule in the system.
/// This entity stores the business rules for discounts based on quantity ranges.
/// </summary>
public class DiscountRule : BaseEntity
{
    /// <summary>
    /// Gets or sets the minimum quantity required for this discount rule
    /// </summary>
    public int MinQuantity { get; set; }

    /// <summary>
    /// Gets or sets the maximum quantity allowed for this discount rule
    /// </summary>
    public int MaxQuantity { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage (e.g., 10 for 10%, 20 for 20%)
    /// </summary>
    public decimal DiscountPercentage { get; set; }

    /// <summary>
    /// Gets or sets whether this discount rule is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the rule was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the rule was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the DiscountRule class
    /// </summary>
    public DiscountRule()
    {
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    /// <summary>
    /// Initializes a new instance of the DiscountRule class with required parameters
    /// </summary>
    /// <param name="minQuantity">The minimum quantity</param>
    /// <param name="maxQuantity">The maximum quantity</param>
    /// <param name="discountPercentage">The discount percentage</param>
    public DiscountRule(int minQuantity, int maxQuantity, decimal discountPercentage)
    {
        MinQuantity = minQuantity;
        MaxQuantity = maxQuantity;
        DiscountPercentage = discountPercentage;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Checks if a given quantity falls within this rule's range
    /// </summary>
    /// <param name="quantity">The quantity to check</param>
    /// <returns>True if the quantity is within the range, false otherwise</returns>
    public bool AppliesTo(int quantity)
    {
        return IsActive && quantity >= MinQuantity && quantity <= MaxQuantity;
    }

    /// <summary>
    /// Activates this discount rule
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates this discount rule
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the discount rule parameters
    /// </summary>
    /// <param name="minQuantity">The new minimum quantity</param>
    /// <param name="maxQuantity">The new maximum quantity</param>
    /// <param name="discountPercentage">The new discount percentage</param>
    public void Update(int minQuantity, int maxQuantity, decimal discountPercentage)
    {
        MinQuantity = minQuantity;
        MaxQuantity = maxQuantity;
        DiscountPercentage = discountPercentage;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Performs validation of the discount rule entity
    /// </summary>
    /// <returns>A ValidationResultDetail containing validation results</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new DiscountRuleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

