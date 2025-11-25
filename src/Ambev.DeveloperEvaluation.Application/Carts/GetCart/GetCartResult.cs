namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

/// <summary>
/// Result for GetCart operation
/// </summary>
public class GetCartResult
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
    public List<CartProductResultDto> Products { get; set; } = new();
}

/// <summary>
/// DTO for cart product result
/// </summary>
public class CartProductResultDto
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

