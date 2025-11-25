namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

/// <summary>
/// Response model for cart product
/// </summary>
public class CartProductResponse
{
    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity
    /// </summary>
    public int Quantity { get; set; }
}

