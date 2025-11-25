using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class GetCartsRequestProfile : Profile
{
    public GetCartsRequestProfile()
    {
        CreateMap<GetCartsResult, GetCartsResponse>();
        CreateMap<CartDto, CartListItemDto>();
        CreateMap<CartProductDto, CartProductResponse>();
    }
}

