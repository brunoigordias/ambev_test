using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

/// <summary>
/// Response model for cart update
/// </summary>
public class UpdateCartResponse
{
    /// <summary>
    /// Gets or sets the cart ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the cart date
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the list of products in the cart
    /// </summary>
    public List<CartProductResponse> Products { get; set; } = new();
}

