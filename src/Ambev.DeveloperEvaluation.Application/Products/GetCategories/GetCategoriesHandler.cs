using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.GetCategories;

/// <summary>
/// Handler for processing GetCategoriesQuery requests
/// </summary>
public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, GetCategoriesResult>
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of GetCategoriesHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    public GetCategoriesHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the GetCategoriesQuery request
    /// </summary>
    /// <param name="query">The GetCategories query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of categories</returns>
    public async Task<GetCategoriesResult> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await _productRepository.GetCategoriesAsync(cancellationToken);

        return new GetCategoriesResult
        {
            Categories = categories.ToList()
        };
    }
}



