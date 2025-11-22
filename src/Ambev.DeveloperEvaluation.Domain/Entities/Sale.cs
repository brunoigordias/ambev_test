using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale record in the system.
/// This entity follows domain-driven design principles and includes business rules validation.
/// Uses External Identities pattern with denormalization for Customer and Branch references.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets or sets the sale number (unique identifier for business purposes)
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the sale was made
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the external customer ID (External Identity pattern)
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the customer name (denormalized from external domain)
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the external branch ID (External Identity pattern)
    /// </summary>
    public int BranchId { get; set; }

    /// <summary>
    /// Gets or sets the branch description (denormalized from external domain)
    /// </summary>
    public string BranchDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total sale amount
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the sale status (Active or Cancelled)
    /// </summary>
    public SaleStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the sale was cancelled
    /// </summary>
    public DateTime? CancelledAt { get; set; }

    /// <summary>
    /// Gets or sets the list of items in this sale
    /// </summary>
    public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();

    /// <summary>
    /// Gets or sets the date and time when the sale was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the last update to the sale
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the Sale class
    /// </summary>
    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        Status = SaleStatus.Active;
        SaleDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds an item to the sale
    /// </summary>
    /// <param name="productId">The external product ID</param>
    /// <param name="productDescription">The product description</param>
    /// <param name="quantity">The quantity</param>
    /// <param name="unitPrice">The unit price</param>
    public void AddItem(int productId, string productDescription, int quantity, decimal unitPrice)
    {
        if (Status == SaleStatus.Cancelled)
        {
            throw new DomainException("Cannot add items to a cancelled sale.");
        }

        var item = new SaleItem(productId, productDescription, quantity, unitPrice)
        {
            SaleId = Id
        };

        Items.Add(item);
        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates an item in the sale
    /// </summary>
    /// <param name="itemId">The item ID</param>
    /// <param name="quantity">The new quantity</param>
    public void UpdateItem(Guid itemId, int quantity)
    {
        if (Status == SaleStatus.Cancelled)
        {
            throw new DomainException("Cannot update items in a cancelled sale.");
        }

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            throw new DomainException($"Item with ID {itemId} not found in this sale.");
        }

        item.UpdateQuantity(quantity);
        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes an item from the sale
    /// </summary>
    /// <param name="itemId">The item ID</param>
    public void RemoveItem(Guid itemId)
    {
        if (Status == SaleStatus.Cancelled)
        {
            throw new DomainException("Cannot remove items from a cancelled sale.");
        }

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            throw new DomainException($"Item with ID {itemId} not found in this sale.");
        }

        if (item.IsCancelled)
        {
            throw new DomainException("Cannot remove an already cancelled item.");
        }

        Items.Remove(item);
        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels a specific item in the sale
    /// </summary>
    /// <param name="itemId">The item ID</param>
    public void CancelItem(Guid itemId)
    {
        if (Status == SaleStatus.Cancelled)
        {
            throw new DomainException("Cannot cancel items in a cancelled sale.");
        }

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            throw new DomainException($"Item with ID {itemId} not found in this sale.");
        }

        item.Cancel();
        CalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the total amount of the sale based on all items
    /// </summary>
    public void CalculateTotalAmount()
    {
        TotalAmount = Items
            .Where(item => !item.IsCancelled)
            .Sum(item => item.TotalAmount);
    }

    /// <summary>
    /// Cancels the entire sale
    /// </summary>
    public void Cancel()
    {
        if (Status == SaleStatus.Cancelled)
        {
            throw new DomainException("Sale is already cancelled.");
        }

        Status = SaleStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Performs validation of the sale entity using the SaleValidator rules
    /// </summary>
    /// <returns>A ValidationResultDetail containing validation results</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

