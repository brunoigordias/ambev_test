using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Handler for SaleCancelledEvent messages from Rebus
/// </summary>
public class SaleCancelledEventHandler : IHandleMessages<SaleCancelledEvent>
{
    private readonly ILogger<SaleCancelledEventHandler> _logger;
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    /// Initializes a new instance of SaleCancelledEventHandler
    /// </summary>
    public SaleCancelledEventHandler(
        ILogger<SaleCancelledEventHandler> logger,
        ISaleRepository saleRepository)
    {
        _logger = logger;
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Handles the SaleCancelledEvent message
    /// </summary>
    public async Task Handle(SaleCancelledEvent message)
    {
        try
        {
            _logger.LogInformation(
                "Processando SaleCancelledEvent - SaleId: {SaleId}, SaleNumber: {SaleNumber}, Customer: {CustomerName}",
                message.Sale.Id,
                message.Sale.SaleNumber,
                message.Sale.CustomerName);

            // Validação: Verificar se a venda existe no banco
            var sale = await _saleRepository.GetByIdAsync(message.Sale.Id);
            if (sale == null)
            {
                _logger.LogWarning(
                    "Venda não encontrada no banco de dados - SaleId: {SaleId}, SaleNumber: {SaleNumber}",
                    message.Sale.Id,
                    message.Sale.SaleNumber);
                throw new ArgumentException($"Venda não encontrada: {message.Sale.Id}");
            }

            // Validação: Verificar se a venda está realmente cancelada
            if (sale.Status != Domain.Enums.SaleStatus.Cancelled)
            {
                _logger.LogWarning(
                    "Venda não está cancelada - SaleId: {SaleId}, Status atual: {Status}",
                    message.Sale.Id,
                    sale.Status);
            }           

            _logger.LogInformation(
                "SaleCancelledEvent processado com sucesso - SaleId: {SaleId}, Total cancelado: {Total}",
                message.Sale.Id,
                message.Sale.TotalAmount);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro ao processar SaleCancelledEvent - SaleId: {SaleId}, SaleNumber: {SaleNumber}",
                message.Sale.Id,
                message.Sale.SaleNumber);
            throw;
        }
    }
}

