using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Handler for processing CancelSaleItemCommand requests
/// </summary>
public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, CancelSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IBus _bus;

    public CancelSaleItemHandler(ISaleRepository saleRepository, IMapper mapper, IBus bus)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _bus = bus;
    }

    public async Task<CancelSaleItemResult> Handle(CancelSaleItemCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found");

        sale.CancelItem(command.ItemId);
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        var cancelledItem = updatedSale.Items.FirstOrDefault(i => i.Id == command.ItemId);

        // Publish ItemCancelledEvent via Rebus
        if (cancelledItem != null)
        {
            var itemCancelledEvent = new ItemCancelledEvent(cancelledItem, updatedSale);
            await _bus.Publish(itemCancelledEvent);
        }

        return new CancelSaleItemResult
        {
            SaleId = updatedSale.Id,
            ItemId = command.ItemId,
            NewTotalAmount = updatedSale.TotalAmount
        };
    }
}

