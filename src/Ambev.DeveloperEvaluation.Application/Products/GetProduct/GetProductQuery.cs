using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Query for retrieving a product by ID
/// </summary>
public class GetProductQuery : IRequest<GetProductResult>
{
    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public Guid Id { get; set; }
}


