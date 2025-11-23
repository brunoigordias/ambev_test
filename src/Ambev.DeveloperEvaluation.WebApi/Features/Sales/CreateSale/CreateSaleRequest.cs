namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Request DTO for creating a new sale
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sale date
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the external customer ID
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the customer name
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the external branch ID
    /// </summary>
    public int BranchId { get; set; }

    /// <summary>
    /// Gets or sets the branch description
    /// </summary>
    public string BranchDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of items in the sale
    /// </summary>
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}

