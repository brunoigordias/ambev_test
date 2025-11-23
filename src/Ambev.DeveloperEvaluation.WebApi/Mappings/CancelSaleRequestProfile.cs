using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class CancelSaleRequestProfile : Profile
{
    public CancelSaleRequestProfile()
    {
        CreateMap<CancelSaleRequest, CancelSaleCommand>()
            .ConstructUsing(req => new CancelSaleCommand(req.Id));
        CreateMap<CancelSaleResult, CancelSaleResponse>();
    }
}

