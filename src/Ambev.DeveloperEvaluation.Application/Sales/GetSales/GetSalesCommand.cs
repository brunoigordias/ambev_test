using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Command for retrieving a list of sales with pagination and filtering
/// </summary>
public class GetSalesCommand : IRequest<GetSalesResult>
{
    /// <summary>
    /// Gets or sets the page number (default: 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size (default: 10)
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the ordering (e.g., "saleDate desc, totalAmount asc")
    /// </summary>
    public string? Order { get; set; }

    /// <summary>
    /// Gets or sets the customer ID filter
    /// </summary>
    public int? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the branch ID filter
    /// </summary>
    public int? BranchId { get; set; }

    /// <summary>
    /// Gets or sets the status filter
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the minimum sale date filter
    /// </summary>
    public DateTime? MinSaleDate { get; set; }

    /// <summary>
    /// Gets or sets the maximum sale date filter
    /// </summary>
    public DateTime? MaxSaleDate { get; set; }
}

