namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

/// <summary>
/// Rating request model
/// </summary>
public class ProductRatingRequest
{
    /// <summary>
    /// Gets or sets the rating rate
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Gets or sets the rating count
    /// </summary>
    public int Count { get; set; }
}

/// <summary>
/// Rating response model
/// </summary>
public class ProductRatingResponse
{
    /// <summary>
    /// Gets or sets the rating rate
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Gets or sets the rating count
    /// </summary>
    public int Count { get; set; }
}



