using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Profile for mapping CreateProduct operations
/// </summary>
public class CreateProductProfile : Profile
{
    public CreateProductProfile()
    {
        CreateMap<CreateProductCommand, Product>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new Rating(src.RatingRate, src.RatingCount)));

        CreateMap<Product, CreateProductResult>()
            .ForMember(dest => dest.RatingRate, opt => opt.MapFrom(src => src.Rating.Rate))
            .ForMember(dest => dest.RatingCount, opt => opt.MapFrom(src => src.Rating.Count));
    }
}


