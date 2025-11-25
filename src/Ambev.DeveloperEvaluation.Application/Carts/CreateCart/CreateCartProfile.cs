using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

/// <summary>
/// Profile for mapping CreateCart operations
/// </summary>
public class CreateCartProfile : Profile
{
    public CreateCartProfile()
    {
        CreateMap<CreateCartCommand, Cart>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(p => new CartItem(p.ProductId, p.Quantity))));

        CreateMap<Cart, CreateCartResult>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(p => new CartProductResultDto
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity
            })));
    }
}

