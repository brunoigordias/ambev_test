using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a cart in the system.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Cart : BaseEntity
{
    /// <summary>
    /// Gets or sets the user ID who owns this cart
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the date of the cart
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the list of items in this cart
    /// </summary>
    public ICollection<CartItem> Products { get; set; } = new List<CartItem>();

    /// <summary>
    /// Gets or sets the date and time when the cart was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the last update to the cart
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the Cart class
    /// </summary>
    public Cart()
    {
        CreatedAt = DateTime.UtcNow;
        Date = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a product to the cart
    /// </summary>
    /// <param name="productId">The product ID</param>
    /// <param name="quantity">The quantity</param>
    public void AddProduct(int productId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        var existingItem = Products.FirstOrDefault(p => p.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var item = new CartItem(productId, quantity)
            {
                CartId = Id
            };
            Products.Add(item);
        }

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a product from the cart
    /// </summary>
    /// <param name="productId">The product ID to remove</param>
    public void RemoveProduct(int productId)
    {
        var item = Products.FirstOrDefault(p => p.ProductId == productId);
        if (item != null)
        {
            Products.Remove(item);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Updates the quantity of a product in the cart
    /// </summary>
    /// <param name="productId">The product ID</param>
    /// <param name="quantity">The new quantity</param>
    public void UpdateProductQuantity(int productId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        var item = Products.FirstOrDefault(p => p.ProductId == productId);
        if (item == null)
            throw new InvalidOperationException($"Product {productId} not found in cart");

        item.UpdateQuantity(quantity);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Clears all products from the cart
    /// </summary>
    public void Clear()
    {
        Products.Clear();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the cart information
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="date">The cart date</param>
    public void Update(int userId, DateTime date)
    {
        UserId = userId;
        Date = date;
        UpdatedAt = DateTime.UtcNow;
    }
}

