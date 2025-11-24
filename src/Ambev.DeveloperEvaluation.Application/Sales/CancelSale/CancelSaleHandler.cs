using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for processing CancelSaleCommand requests
/// </summary>
public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleHandler> _logger;

    public CancelSaleHandler(
        ISaleRepository saleRepository, 
        IMapper mapper,
        ILogger<CancelSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        sale.Cancel();
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Log SaleCancelledEvent
        var saleCancelledEvent = new SaleCancelledEvent(updatedSale);
        _logger.LogInformation(
            "SaleCancelledEvent: Sale {SaleId} with number {SaleNumber} cancelled at {CancelledAt}. Customer: {CustomerName} ({CustomerId}), Branch: {BranchDescription} ({BranchId}). Original amount: {TotalAmount:C}",
            updatedSale.Id,
            updatedSale.SaleNumber,
            updatedSale.CancelledAt,
            updatedSale.CustomerName,
            updatedSale.CustomerId,
            updatedSale.BranchDescription,
            updatedSale.BranchId,
            updatedSale.TotalAmount);

        return _mapper.Map<CancelSaleResult>(updatedSale);
    }
}

