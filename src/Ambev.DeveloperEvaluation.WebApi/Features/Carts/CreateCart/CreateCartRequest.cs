using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

/// <summary>
/// Request model for creating a new cart
/// </summary>
public class CreateCartRequest
{
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
    public List<CartProductRequest> Products { get; set; } = new();
}

