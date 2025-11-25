using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;

/// <summary>
/// Response model for getting carts list
/// </summary>
public class GetCartsResponse
{
    /// <summary>
    /// Gets or sets the list of carts
    /// </summary>
    public List<CartListItemDto> Data { get; set; } = new();

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
/// Cart DTO for list response
/// </summary>
public class CartListItemDto
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

