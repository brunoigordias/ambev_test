using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class UpdateCartRequestProfile : Profile
{
    public UpdateCartRequestProfile()
    {
        CreateMap<UpdateCartRequest, UpdateCartCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateCartResult, UpdateCartResponse>();
    }
}

