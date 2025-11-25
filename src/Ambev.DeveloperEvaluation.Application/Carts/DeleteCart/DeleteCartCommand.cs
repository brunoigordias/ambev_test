using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

/// <summary>
/// Command for deleting a cart
/// </summary>
public class DeleteCartCommand : IRequest<DeleteCartResult>
{
    /// <summary>
    /// Gets or sets the cart ID
    /// </summary>
    public Guid Id { get; set; }
}

