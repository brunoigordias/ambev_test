using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;

/// <summary>
/// Query for retrieving products by category
/// </summary>
public class GetProductsByCategoryQuery : IRequest<GetProductsByCategoryResult>
{
    /// <summary>
    /// Gets or sets the category name
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the page number (1-based)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the order by expression
    /// </summary>
    public string? Order { get; set; }
}


