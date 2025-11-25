namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;

/// <summary>
/// Response model for product deletion
/// </summary>
public class DeleteProductResponse
{
    /// <summary>
    /// Gets or sets whether the deletion was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the message
    /// </summary>
    public string Message { get; set; } = string.Empty;
}



