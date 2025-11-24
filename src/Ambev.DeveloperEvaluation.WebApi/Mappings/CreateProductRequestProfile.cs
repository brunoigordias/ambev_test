using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class CreateProductRequestProfile : Profile
{
    public CreateProductRequestProfile()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>()
            .ForMember(dest => dest.RatingRate, opt => opt.MapFrom(src => src.Rating.Rate))
            .ForMember(dest => dest.RatingCount, opt => opt.MapFrom(src => src.Rating.Count));

        CreateMap<CreateProductResult, CreateProductResponse>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRatingResponse
            {
                Rate = src.RatingRate,
                Count = src.RatingCount
            }));
    }
}


