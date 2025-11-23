namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

/// <summary>
/// Response DTO returned after cancelling a sale item
/// </summary>
public class CancelSaleItemResponse
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
    public decimal NewTotalAmount { get; set; }
}

