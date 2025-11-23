using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class CancelSaleItemRequestProfile : Profile
{
    public CancelSaleItemRequestProfile()
    {
        CreateMap<CancelSaleItemRequest, CancelSaleItemCommand>();
        CreateMap<CancelSaleItemResult, CancelSaleItemResponse>();
    }
}

