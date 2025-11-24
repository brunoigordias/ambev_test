using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Handler for ItemCancelledEvent messages from Rebus
/// </summary>
public class ItemCancelledEventHandler : IHandleMessages<ItemCancelledEvent>
{
    private readonly ILogger<ItemCancelledEventHandler> _logger;
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    /// Initializes a new instance of ItemCancelledEventHandler
    /// </summary>
    public ItemCancelledEventHandler(
        ILogger<ItemCancelledEventHandler> logger,
        ISaleRepository saleRepository)
    {
        _logger = logger;
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Handles the ItemCancelledEvent message
    /// </summary>
    public async Task Handle(ItemCancelledEvent message)
    {
        try
        {
            _logger.LogInformation(
                "Processando ItemCancelledEvent - SaleId: {SaleId}, SaleNumber: {SaleNumber}, ProductId: {ProductId}, ProductDescription: {ProductDescription}, Quantity: {Quantity}",
                message.Sale.Id,
                message.Sale.SaleNumber,
                message.Item.ProductId,
                message.Item.ProductDescription,
                message.Item.Quantity);

            // Validação: Verificar se a venda existe no banco
            var sale = await _saleRepository.GetByIdAsync(message.Sale.Id);
            if (sale == null)
            {
                _logger.LogWarning(
                    "Venda não encontrada no banco de dados - SaleId: {SaleId}",
                    message.Sale.Id);
                throw new ArgumentException($"Venda não encontrada: {message.Sale.Id}");
            }

            // Validação: Verificar se o item existe na venda
            var item = sale.Items.FirstOrDefault(i => i.Id == message.Item.Id);
            if (item == null)
            {
                _logger.LogWarning(
                    "Item não encontrado na venda - SaleId: {SaleId}, ItemId: {ItemId}",
                    message.Sale.Id,
                    message.Item.Id);
                throw new ArgumentException($"Item não encontrado na venda: {message.Item.Id}");
            }

            // Validação: Verificar se a quantidade cancelada é válida
            if (message.Item.Quantity <= 0)
            {
                _logger.LogWarning(
                    "Quantidade cancelada inválida - SaleId: {SaleId}, ItemId: {ItemId}, Quantity: {Quantity}",
                    message.Sale.Id,
                    message.Item.Id,
                    message.Item.Quantity);
                throw new ArgumentException("A quantidade cancelada deve ser maior que zero");
            }            

            _logger.LogInformation(
                "ItemCancelledEvent processado com sucesso - SaleId: {SaleId}, ItemId: {ItemId}, Valor cancelado: {ItemTotal}",
                message.Sale.Id,
                message.Item.Id,
                message.Item.TotalAmount);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro ao processar ItemCancelledEvent - SaleId: {SaleId}, ItemId: {ItemId}",
                message.Sale.Id,
                message.Item.Id);
            throw;
        }
    }
}

