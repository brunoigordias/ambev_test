using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Request DTO for retrieving a list of sales with pagination and filtering
/// </summary>
public class GetSalesRequest
{
    /// <summary>
    /// Gets or sets the page number (default: 1)
    /// Query parameter: _page
    /// </summary>
    [FromQuery(Name = "_page")]
    public int? _page { get; set; }

    /// <summary>
    /// Gets or sets the page size (default: 10)
    /// Query parameter: _size
    /// </summary>
    [FromQuery(Name = "_size")]
    public int? _size { get; set; }

    /// <summary>
    /// Gets or sets the ordering (e.g., "saleDate desc, totalAmount asc")
    /// Query parameter: _order
    /// </summary>
    [FromQuery(Name = "_order")]
    public string? _order { get; set; }

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

