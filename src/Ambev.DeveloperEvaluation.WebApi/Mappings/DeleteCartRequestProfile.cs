using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class DeleteCartRequestProfile : Profile
{
    public DeleteCartRequestProfile()
    {
        CreateMap<DeleteCartResult, DeleteCartResponse>();
    }
}

