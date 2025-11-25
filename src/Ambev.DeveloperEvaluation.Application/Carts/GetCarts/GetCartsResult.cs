namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

/// <summary>
/// Result for GetCarts operation
/// </summary>
public class GetCartsResult
{
    /// <summary>
    /// Gets or sets the list of carts
    /// </summary>
    public List<CartDto> Data { get; set; } = new();

    /// <summary>
    /// Gets or sets the total number of items
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Gets or sets the current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages
    /// </summary>
    public int TotalPages { get; set; }
}

/// <summary>
/// DTO for cart
/// </summary>
public class CartDto
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
    public List<CartProductDto> Products { get; set; } = new();
}

/// <summary>
/// DTO for cart product
/// </summary>
public class CartProductDto
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

