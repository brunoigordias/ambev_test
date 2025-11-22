using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a sale.
/// Each sale item contains product information (denormalized from external domain),
/// quantity, unit price, discount, and calculated total amount.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the sale ID this item belongs to
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the sale this item belongs to
    /// </summary>
    public Sale? Sale { get; set; }

    /// <summary>
    /// Gets or sets the external product ID (External Identity pattern)
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product description (denormalized from external domain)
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of items
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage applied to this item (0-100)
    /// </summary>
    public decimal DiscountPercentage { get; set; }

    /// <summary>
    /// Gets or sets the discount amount calculated
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this item (after discount)
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether this item is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the item was cancelled
    /// </summary>
    public DateTime? CancelledAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the SaleItem class
    /// </summary>
    public SaleItem()
    {
    }

    /// <summary>
    /// Initializes a new instance of the SaleItem class with required parameters
    /// </summary>
    /// <param name="productId">The external product ID</param>
    /// <param name="productDescription">The product description</param>
    /// <param name="quantity">The quantity of items</param>
    /// <param name="unitPrice">The unit price</param>
    public SaleItem(int productId, string productDescription, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        ProductDescription = productDescription;
        Quantity = quantity;
        UnitPrice = unitPrice;
        IsCancelled = false;
        CalculateDiscountAndTotal();
    }

    /// <summary>
    /// Calculates the discount and total amount based on quantity and business rules
    /// Business rules:
    /// - 4+ items: 10% discount
    /// - 10-20 items: 20% discount
    /// - Below 4 items: no discount
    /// - Maximum 20 items per product
    /// </summary>
    public void CalculateDiscountAndTotal()
    {
        // Business rule: Maximum 20 items per product
        if (Quantity > 20)
        {
            throw new DomainException("It's not possible to sell above 20 identical items.");
        }

        // Business rule: Below 4 items cannot have discount
        if (Quantity < 4)
        {
            DiscountPercentage = 0;
        }
        // Business rule: 10-20 items have 20% discount
        else if (Quantity >= 10 && Quantity <= 20)
        {
            DiscountPercentage = 20;
        }
        // Business rule: 4+ items have 10% discount
        else if (Quantity >= 4)
        {
            DiscountPercentage = 10;
        }

        var subtotal = Quantity * UnitPrice;
        DiscountAmount = subtotal * (DiscountPercentage / 100);
        TotalAmount = subtotal - DiscountAmount;
    }

    /// <summary>
    /// Updates the quantity and recalculates discount and total
    /// </summary>
    /// <param name="newQuantity">The new quantity</param>
    public void UpdateQuantity(int newQuantity)
    {
        if (IsCancelled)
        {
            throw new DomainException("Cannot update a cancelled item.");
        }

        Quantity = newQuantity;
        CalculateDiscountAndTotal();
    }

    /// <summary>
    /// Cancels this sale item
    /// </summary>
    public void Cancel()
    {
        if (IsCancelled)
        {
            throw new DomainException("Item is already cancelled.");
        }

        IsCancelled = true;
        CancelledAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Performs validation of the sale item entity
    /// </summary>
    /// <returns>A ValidationResultDetail containing validation results</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

