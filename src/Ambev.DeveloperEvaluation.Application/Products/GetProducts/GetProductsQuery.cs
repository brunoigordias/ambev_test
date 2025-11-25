using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Query for retrieving paginated list of products
/// </summary>
public class GetProductsQuery : IRequest<GetProductsResult>
{
    /// <summary>
    /// Gets or sets the page number (1-based)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the order by expression (e.g., "price desc, title asc")
    /// </summary>
    public string? Order { get; set; }

    /// <summary>
    /// Gets or sets filters to apply
    /// </summary>
    public Dictionary<string, string>? Filters { get; set; }
}



