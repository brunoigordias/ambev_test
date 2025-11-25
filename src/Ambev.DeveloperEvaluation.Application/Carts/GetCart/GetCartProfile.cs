using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

/// <summary>
/// Profile for mapping GetCart operations
/// </summary>
public class GetCartProfile : Profile
{
    public GetCartProfile()
    {
        CreateMap<Domain.Entities.Cart, GetCartResult>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(p => new CartProductResultDto
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity
            })));
    }
}

