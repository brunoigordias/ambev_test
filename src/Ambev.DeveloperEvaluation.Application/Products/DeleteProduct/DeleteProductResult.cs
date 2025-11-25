namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Result for DeleteProduct operation
/// </summary>
public class DeleteProductResult
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



