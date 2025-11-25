using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

/// <summary>
/// Query for retrieving a cart by ID
/// </summary>
public class GetCartQuery : IRequest<GetCartResult>
{
    /// <summary>
    /// Gets or sets the cart ID
    /// </summary>
    public Guid Id { get; set; }
}

