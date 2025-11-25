using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

/// <summary>
/// Profile for mapping UpdateCart operations
/// </summary>
public class UpdateCartProfile : Profile
{
    public UpdateCartProfile()
    {
        CreateMap<Domain.Entities.Cart, UpdateCartResult>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(p => new CartProductResultDto
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity
            })));
    }
}

