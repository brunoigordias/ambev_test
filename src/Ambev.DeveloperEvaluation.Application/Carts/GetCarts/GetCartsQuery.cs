using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

/// <summary>
/// Query for retrieving all carts with pagination and filtering
/// </summary>
public class GetCartsQuery : IRequest<GetCartsResult>
{
    /// <summary>
    /// Gets or sets the page number
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the order by expression
    /// </summary>
    public string? Order { get; set; }

    /// <summary>
    /// Gets or sets the filters dictionary
    /// </summary>
    public Dictionary<string, string>? Filters { get; set; }
}

