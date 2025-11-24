using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Products.GetProducts;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;

/// <summary>
/// Handler for processing GetProductsByCategoryQuery requests
/// </summary>
public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetProductsByCategoryHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetProductsByCategoryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetProductsByCategoryQuery request
    /// </summary>
    /// <param name="query">The GetProductsByCategory query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of products in the category</returns>
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var (products, totalCount) = await _productRepository.GetByCategoryAsync(
            query.Category,
            query.Page,
            query.Size,
            query.Order,
            cancellationToken);

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        var totalPages = (int)Math.Ceiling(totalCount / (double)query.Size);

        return new GetProductsByCategoryResult
        {
            Data = productDtos,
            TotalItems = totalCount,
            CurrentPage = query.Page,
            TotalPages = totalPages
        };
    }
}


