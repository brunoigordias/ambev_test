namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Response model for getting products list
/// </summary>
public class GetProductsResponse
{
    /// <summary>
    /// Gets or sets the list of products
    /// </summary>
    public List<ProductListItemDto> Data { get; set; } = new();

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
/// Product DTO for list response
/// </summary>
public class ProductListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public ProductListRatingDto Rating { get; set; } = new();
}

/// <summary>
/// Rating DTO for response
/// </summary>
public class ProductListRatingDto
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}

