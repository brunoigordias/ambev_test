using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a cart.
/// Each cart item contains product information and quantity.
/// </summary>
public class CartItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the cart ID this item belongs to
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Gets or sets the cart this item belongs to
    /// </summary>
    public Cart? Cart { get; set; }

    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of items
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Initializes a new instance of the CartItem class
    /// </summary>
    public CartItem()
    {
    }

    /// <summary>
    /// Initializes a new instance of the CartItem class with product ID and quantity
    /// </summary>
    /// <param name="productId">The product ID</param>
    /// <param name="quantity">The quantity</param>
    public CartItem(int productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

    /// <summary>
    /// Updates the quantity of the cart item
    /// </summary>
    /// <param name="quantity">The new quantity</param>
    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        Quantity = quantity;
    }
}

