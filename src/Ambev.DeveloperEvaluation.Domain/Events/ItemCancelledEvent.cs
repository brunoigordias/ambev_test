using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a sale item is cancelled
/// </summary>
public class ItemCancelledEvent
{
    /// <summary>
    /// Gets the sale item that was cancelled
    /// </summary>
    public SaleItem Item { get; }

    /// <summary>
    /// Gets the sale that contains the cancelled item
    /// </summary>
    public Sale Sale { get; }

    /// <summary>
    /// Initializes a new instance of ItemCancelledEvent
    /// </summary>
    /// <param name="item">The sale item that was cancelled</param>
    /// <param name="sale">The sale that contains the cancelled item</param>
    public ItemCancelledEvent(SaleItem item, Sale sale)
    {
        Item = item;
        Sale = sale;
    }
}

