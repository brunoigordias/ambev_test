namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Result for CreateProduct operation
/// </summary>
public class CreateProductResult
{
    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public Guid Id { get; set; }

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
    /// Gets or sets the product rating rate
    /// </summary>
    public decimal RatingRate { get; set; }

    /// <summary>
    /// Gets or sets the product rating count
    /// </summary>
    public int RatingCount { get; set; }
}


