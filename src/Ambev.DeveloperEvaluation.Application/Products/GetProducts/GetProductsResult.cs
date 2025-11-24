namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Result for GetProducts operation
/// </summary>
public class GetProductsResult
{
    /// <summary>
    /// Gets or sets the list of products
    /// </summary>
    public List<ProductDto> Data { get; set; } = new();

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
/// DTO for Product in list
/// </summary>
public class ProductDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public RatingDto Rating { get; set; } = new();
}

/// <summary>
/// DTO for Rating
/// </summary>
public class RatingDto
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}


