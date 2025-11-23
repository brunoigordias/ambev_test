using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handler for processing GetSalesCommand requests
/// </summary>
public class GetSalesHandler : IRequestHandler<GetSalesCommand, GetSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSalesHandler
    /// </summary>
    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetSalesCommand request
    /// </summary>
    public async Task<GetSalesResult> Handle(GetSalesCommand command, CancellationToken cancellationToken)
    {
        var query = _saleRepository.GetQueryable();

        // Apply filters
        if (command.CustomerId.HasValue)
        {
            query = query.Where(s => s.CustomerId == command.CustomerId.Value);
        }

        if (command.BranchId.HasValue)
        {
            query = query.Where(s => s.BranchId == command.BranchId.Value);
        }

        if (!string.IsNullOrEmpty(command.Status))
        {
            if (Enum.TryParse<SaleStatus>(command.Status, out var status))
            {
                query = query.Where(s => s.Status == status);
            }
        }

        if (command.MinSaleDate.HasValue)
        {
            query = query.Where(s => s.SaleDate >= command.MinSaleDate.Value);
        }

        if (command.MaxSaleDate.HasValue)
        {
            query = query.Where(s => s.SaleDate <= command.MaxSaleDate.Value);
        }

        // Apply ordering
        if (!string.IsNullOrEmpty(command.Order))
        {
            // Simple ordering implementation - can be enhanced
            var orderParts = command.Order.Split(',');
            foreach (var part in orderParts)
            {
                var trimmed = part.Trim();
                if (trimmed.StartsWith("saleDate", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(s => s.SaleDate)
                        : query.OrderBy(s => s.SaleDate);
                }
                else if (trimmed.StartsWith("totalAmount", StringComparison.OrdinalIgnoreCase))
                {
                    query = trimmed.EndsWith("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(s => s.TotalAmount)
                        : query.OrderBy(s => s.TotalAmount);
                }
            }
        }
        else
        {
            query = query.OrderByDescending(s => s.SaleDate);
        }

        // Get total count
        var totalItems = await query.CountAsync(cancellationToken);

        // Apply pagination
        var page = Math.Max(1, command.Page);
        var size = Math.Max(1, Math.Min(100, command.Size)); // Limit to 100 items per page
        var skip = (page - 1) * size;

        var sales = await query
            .Skip(skip)
            .Take(size)
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        return new GetSalesResult
        {
            Data = _mapper.Map<List<GetSaleListItemResult>>(sales),
            TotalItems = totalItems,
            CurrentPage = page,
            TotalPages = totalPages
        };
    }
}

