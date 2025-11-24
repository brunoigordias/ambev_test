using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Handler for processing GetProductsQuery requests
/// </summary>
public class GetProductsHandler : IRequestHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetProductsHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetProductsQuery request
    /// </summary>
    /// <param name="query">The GetProducts query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of products</returns>
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var (products, totalCount) = await _productRepository.GetAllAsync(
            query.Page,
            query.Size,
            query.Order,
            query.Filters,
            cancellationToken);

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        var totalPages = (int)Math.Ceiling(totalCount / (double)query.Size);

        return new GetProductsResult
        {
            Data = productDtos,
            TotalItems = totalCount,
            CurrentPage = query.Page,
            TotalPages = totalPages
        };
    }
}


