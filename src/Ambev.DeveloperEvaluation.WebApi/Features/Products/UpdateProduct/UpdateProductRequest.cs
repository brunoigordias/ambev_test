using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// Request model for updating a product
/// </summary>
public class UpdateProductRequest
{
    /// <summary>
    /// Gets or sets the product title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the product description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product category
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product image URL
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product rating
    /// </summary>
    public ProductRatingRequest Rating { get; set; } = new();
}

