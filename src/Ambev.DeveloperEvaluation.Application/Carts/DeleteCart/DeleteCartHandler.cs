using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

/// <summary>
/// Handler for processing DeleteCartCommand requests
/// </summary>
public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, DeleteCartResult>
{
    private readonly ICartRepository _cartRepository;

    /// <summary>
    /// Initializes a new instance of DeleteCartHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    public DeleteCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    /// <summary>
    /// Handles the DeleteCartCommand request
    /// </summary>
    /// <param name="command">The DeleteCart command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success message</returns>
    public async Task<DeleteCartResult> Handle(DeleteCartCommand command, CancellationToken cancellationToken)
    {
        var deleted = await _cartRepository.DeleteAsync(command.Id, cancellationToken);
        
        if (!deleted)
            throw new KeyNotFoundException($"Cart with ID {command.Id} not found");

        return new DeleteCartResult
        {
            Message = "Cart deleted successfully"
        };
    }
}

