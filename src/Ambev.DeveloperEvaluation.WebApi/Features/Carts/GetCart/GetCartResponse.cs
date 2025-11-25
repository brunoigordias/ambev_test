using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

/// <summary>
/// Response model for getting a cart
/// </summary>
public class GetCartResponse
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

