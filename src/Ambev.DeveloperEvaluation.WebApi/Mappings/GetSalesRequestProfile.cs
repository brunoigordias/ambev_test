using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class GetSalesRequestProfile : Profile
{
    public GetSalesRequestProfile()
    {
        CreateMap<GetSalesRequest, GetSalesCommand>()
            .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src._page ?? 1))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src._size ?? 10))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src._order))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.MinSaleDate, opt => opt.MapFrom(src => src.MinSaleDate))
            .ForMember(dest => dest.MaxSaleDate, opt => opt.MapFrom(src => src.MaxSaleDate));
        CreateMap<GetSalesResult, GetSalesResponse>();
        CreateMap<GetSaleListItemResult, GetSaleListItemResponse>();
    }
}

