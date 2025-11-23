namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Result returned when retrieving a list of sales
/// </summary>
public class GetSalesResult
{
    /// <summary>
    /// Gets or sets the list of sales
    /// </summary>
    public List<GetSaleListItemResult> Data { get; set; } = new();

    /// <summary>
    /// Gets or sets the total number of items
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Gets or sets the current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages
    /// </summary>
    public int TotalPages { get; set; }
}

/// <summary>
/// Result for a sale list item (simplified)
/// </summary>
public class GetSaleListItemResult
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public string BranchDescription { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
}

