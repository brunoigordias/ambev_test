using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class DeleteProductRequestProfile : Profile
{
    public DeleteProductRequestProfile()
    {
        CreateMap<DeleteProductResult, DeleteProductResponse>();
    }
}


