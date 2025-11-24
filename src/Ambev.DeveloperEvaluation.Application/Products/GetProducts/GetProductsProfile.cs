using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Profile for mapping GetProducts operations
/// </summary>
public class GetProductsProfile : Profile
{
    public GetProductsProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<Rating, RatingDto>();
    }
}


