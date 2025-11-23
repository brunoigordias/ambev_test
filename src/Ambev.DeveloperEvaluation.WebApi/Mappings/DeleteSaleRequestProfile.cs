using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class DeleteSaleRequestProfile : Profile
{
    public DeleteSaleRequestProfile()
    {
        CreateMap<DeleteSaleRequest, DeleteSaleCommand>()
            .ConstructUsing(req => new DeleteSaleCommand(req.Id));
    }
}

