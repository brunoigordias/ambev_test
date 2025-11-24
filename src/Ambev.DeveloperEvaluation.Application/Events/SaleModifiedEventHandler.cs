using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Handler for SaleModifiedEvent messages from Rebus
/// </summary>
public class SaleModifiedEventHandler : IHandleMessages<SaleModifiedEvent>
{
    private readonly ILogger<SaleModifiedEventHandler> _logger;
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    /// Initializes a new instance of SaleModifiedEventHandler
    /// </summary>
    public SaleModifiedEventHandler(
        ILogger<SaleModifiedEventHandler> logger,
        ISaleRepository saleRepository)
    {
        _logger = logger;
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Handles the SaleModifiedEvent message
    /// </summary>
    public async Task Handle(SaleModifiedEvent message)
    {
        try
        {
            _logger.LogInformation(
                "Processando SaleModifiedEvent - SaleId: {SaleId}, SaleNumber: {SaleNumber}, Customer: {CustomerName}, NewTotal: {Total}",
                message.Sale.Id,
                message.Sale.SaleNumber,
                message.Sale.CustomerName,
                message.Sale.TotalAmount);

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

            // Validação: Verificar se o total está correto
            if (message.Sale.TotalAmount < 0)
            {
                _logger.LogWarning(
                    "Total da venda é negativo - SaleId: {SaleId}, Total: {Total}",
                    message.Sale.Id,
                    message.Sale.TotalAmount);
                throw new ArgumentException("O total da venda não pode ser negativo");
            }            

            _logger.LogInformation(
                "SaleModifiedEvent processado com sucesso - SaleId: {SaleId}, Total atualizado: {Total}, Itens: {ItemCount}",
                message.Sale.Id,
                message.Sale.TotalAmount,
                sale.Items?.Count ?? 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro ao processar SaleModifiedEvent - SaleId: {SaleId}, SaleNumber: {SaleNumber}",
                message.Sale.Id,
                message.Sale.SaleNumber);
            throw;
        }
    }
}

