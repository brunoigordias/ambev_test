using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Profile for mapping GetProduct operations
/// </summary>
public class GetProductProfile : Profile
{
    public GetProductProfile()
    {
        CreateMap<Product, GetProductResult>()
            .ForMember(dest => dest.RatingRate, opt => opt.MapFrom(src => src.Rating.Rate))
            .ForMember(dest => dest.RatingCount, opt => opt.MapFrom(src => src.Rating.Count));
    }
}


