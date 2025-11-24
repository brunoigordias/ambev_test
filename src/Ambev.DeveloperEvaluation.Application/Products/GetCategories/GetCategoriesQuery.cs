using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetCategories;

/// <summary>
/// Query for retrieving all product categories
/// </summary>
public class GetCategoriesQuery : IRequest<GetCategoriesResult>
{
}


