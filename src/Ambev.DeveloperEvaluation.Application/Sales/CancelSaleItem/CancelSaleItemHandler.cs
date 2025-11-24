using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<CancelSaleItemHandler> _logger;

    public CancelSaleItemHandler(
        ISaleRepository saleRepository, 
        IMapper mapper,
        ILogger<CancelSaleItemHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CancelSaleItemResult> Handle(CancelSaleItemCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found");

        sale.CancelItem(command.ItemId);
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        var cancelledItem = updatedSale.Items.FirstOrDefault(i => i.Id == command.ItemId);

        // Log ItemCancelledEvent
        if (cancelledItem != null)
        {
            var itemCancelledEvent = new ItemCancelledEvent(cancelledItem, updatedSale);
            _logger.LogInformation(
                "ItemCancelledEvent: Item {ItemId} cancelled from sale {SaleId} (number: {SaleNumber}). Product: {ProductDescription} ({ProductId}), Quantity: {Quantity}, Original amount: {TotalAmount:C}. New sale total: {NewSaleTotal:C}",
                cancelledItem.Id,
                updatedSale.Id,
                updatedSale.SaleNumber,
                cancelledItem.ProductDescription,
                cancelledItem.ProductId,
                cancelledItem.Quantity,
                cancelledItem.TotalAmount,
                updatedSale.TotalAmount);
        }

        return new CancelSaleItemResult
        {
            SaleId = updatedSale.Id,
            ItemId = command.ItemId,
            NewTotalAmount = updatedSale.TotalAmount
        };
    }
}

