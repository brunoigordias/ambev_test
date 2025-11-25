using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class GetCartRequestProfile : Profile
{
    public GetCartRequestProfile()
    {
        CreateMap<GetCartResult, GetCartResponse>();
    }
}

