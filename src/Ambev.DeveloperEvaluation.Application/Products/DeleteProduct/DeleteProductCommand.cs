using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Command for deleting a product
/// </summary>
public class DeleteProductCommand : IRequest<DeleteProductResult>
{
    /// <summary>
    /// Gets or sets the product ID to delete
    /// </summary>
    public Guid Id { get; set; }
}



