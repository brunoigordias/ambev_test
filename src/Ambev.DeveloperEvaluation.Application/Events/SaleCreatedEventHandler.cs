using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Handler for SaleCreatedEvent messages from Rebus
/// </summary>
public class SaleCreatedEventHandler : IHandleMessages<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    /// Initializes a new instance of SaleCreatedEventHandler
    /// </summary>
    public SaleCreatedEventHandler(
        ILogger<SaleCreatedEventHandler> logger,
        ISaleRepository saleRepository)
    {
        _logger = logger;
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Handles the SaleCreatedEvent message
    /// </summary>
    public async Task Handle(SaleCreatedEvent message)
    {
        try
        {
            _logger.LogInformation(
                "Processando SaleCreatedEvent - SaleId: {SaleId}, SaleNumber: {SaleNumber}, Customer: {CustomerName}, Total: {Total}",
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

            // Validação: Verificar se a venda tem itens
            if (sale.Items == null || !sale.Items.Any())
            {
                _logger.LogWarning(
                    "Venda criada sem itens - SaleId: {SaleId}, SaleNumber: {SaleNumber}",
                    message.Sale.Id,
                    message.Sale.SaleNumber);
            }           

            _logger.LogInformation(
                "SaleCreatedEvent processado com sucesso - SaleId: {SaleId}, Total de itens: {ItemCount}",
                message.Sale.Id,
                sale.Items?.Count ?? 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro ao processar SaleCreatedEvent - SaleId: {SaleId}, SaleNumber: {SaleNumber}",
                message.Sale.Id,
                message.Sale.SaleNumber);
            throw;
        }
    }
}

