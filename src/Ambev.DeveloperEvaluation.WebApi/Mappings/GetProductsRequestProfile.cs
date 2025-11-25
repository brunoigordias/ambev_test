using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class GetProductsRequestProfile : Profile
{
    public GetProductsRequestProfile()
    {
        CreateMap<GetProductsResult, GetProductsResponse>();
        CreateMap<ProductDto, ProductListItemDto>();
        CreateMap<RatingDto, ProductListRatingDto>();
        
        // Map for GetProductsByCategory
        CreateMap<GetProductsByCategoryResult, GetProductsResponse>();
    }
}



