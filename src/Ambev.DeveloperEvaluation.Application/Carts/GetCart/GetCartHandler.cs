using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

/// <summary>
/// Handler for processing GetCartQuery requests
/// </summary>
public class GetCartHandler : IRequestHandler<GetCartQuery, GetCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetCartHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetCartHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetCartQuery request
    /// </summary>
    /// <param name="query">The GetCart query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart details</returns>
    public async Task<GetCartResult> Handle(GetCartQuery query, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(query.Id, cancellationToken);
        
        if (cart == null)
            throw new KeyNotFoundException($"Cart with ID {query.Id} not found");

        return _mapper.Map<GetCartResult>(cart);
    }
}

