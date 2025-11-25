using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

/// <summary>
/// Handler for processing GetCartsQuery requests
/// </summary>
public class GetCartsHandler : IRequestHandler<GetCartsQuery, GetCartsResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetCartsHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetCartsHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetCartsQuery request
    /// </summary>
    /// <param name="query">The GetCarts query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of carts</returns>
    public async Task<GetCartsResult> Handle(GetCartsQuery query, CancellationToken cancellationToken)
    {
        var (carts, totalCount) = await _cartRepository.GetAllAsync(
            query.Page,
            query.Size,
            query.Order,
            query.Filters,
            cancellationToken);

        var cartDtos = _mapper.Map<List<CartDto>>(carts);
        var totalPages = (int)Math.Ceiling(totalCount / (double)query.Size);

        return new GetCartsResult
        {
            Data = cartDtos,
            TotalItems = totalCount,
            CurrentPage = query.Page,
            TotalPages = totalPages
        };
    }
}

