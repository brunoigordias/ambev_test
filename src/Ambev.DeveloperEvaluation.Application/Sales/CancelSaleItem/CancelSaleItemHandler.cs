using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Handler for processing CancelSaleItemCommand requests
/// </summary>
public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, CancelSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public CancelSaleItemHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<CancelSaleItemResult> Handle(CancelSaleItemCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found");

        sale.CancelItem(command.ItemId);
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        var cancelledItem = updatedSale.Items.FirstOrDefault(i => i.Id == command.ItemId);

        // TODO: Publish ItemCancelledEvent
        // if (cancelledItem != null)
        // {
        //     var itemCancelledEvent = new ItemCancelledEvent(cancelledItem, updatedSale);
        //     await _mediator.Publish(itemCancelledEvent, cancellationToken);
        // }

        return new CancelSaleItemResult
        {
            SaleId = updatedSale.Id,
            ItemId = command.ItemId,
            NewTotalAmount = updatedSale.TotalAmount
        };
    }
}

