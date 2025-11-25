using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

/// <summary>
/// Profile for mapping GetCarts operations
/// </summary>
public class GetCartsProfile : Profile
{
    public GetCartsProfile()
    {
        CreateMap<Domain.Entities.Cart, CartDto>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(p => new CartProductDto
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity
            })));
    }
}

