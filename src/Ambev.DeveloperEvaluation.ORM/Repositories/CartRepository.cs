using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ICartRepository using Entity Framework Core
/// </summary>
public class CartRepository : ICartRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of CartRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public CartRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new cart in the database
    /// </summary>
    /// <param name="cart">The cart to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created cart</returns>
    public async Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        await _context.Carts.AddAsync(cart, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return cart;
    }

    /// <summary>
    /// Retrieves a cart by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the cart</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart if found, null otherwise</returns>
    public async Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Carts
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves all carts with optional filtering, pagination and ordering
    /// </summary>
    public async Task<(IEnumerable<Cart> Carts, int TotalCount)> GetAllAsync(
        int page = 1,
        int size = 10,
        string? orderBy = null,
        Dictionary<string, string>? filters = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Carts
            .Include(c => c.Products)
            .AsQueryable();

        // Apply filters
        if (filters != null && filters.Any())
        {
            query = ApplyFilters(query, filters);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply ordering
        if (!string.IsNullOrEmpty(orderBy))
        {
            query = ApplyOrdering(query, orderBy);
        }
        else
        {
            query = query.OrderByDescending(c => c.Date);
        }

        // Apply pagination
        var carts = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (carts, totalCount);
    }

    /// <summary>
    /// Updates an existing cart
    /// </summary>
    /// <param name="cart">The cart to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated cart</returns>
    public async Task<Cart> UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync(cancellationToken);
        return cart;
    }

    /// <summary>
    /// Deletes a cart by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the cart to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the cart was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cart = await GetByIdAsync(id, cancellationToken);
        if (cart == null)
            return false;

        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Applies filters to the query based on the provided dictionary
    /// </summary>
    private IQueryable<Cart> ApplyFilters(IQueryable<Cart> query, Dictionary<string, string> filters)
    {
        foreach (var filter in filters)
        {
            var key = filter.Key.ToLower();
            var value = filter.Value;

            switch (key)
            {
                case "userid":
                    if (int.TryParse(value, out var userId))
                        query = query.Where(c => c.UserId == userId);
                    break;

                case "date":
                    if (DateTime.TryParse(value, out var date))
                        query = query.Where(c => c.Date.Date == date.Date);
                    break;
            }
        }

        return query;
    }

    /// <summary>
    /// Applies ordering to the query based on the provided order string
    /// Format: "field1 asc, field2 desc" or "field1, field2 desc"
    /// </summary>
    private IQueryable<Cart> ApplyOrdering(IQueryable<Cart> query, string orderBy)
    {
        var orderParts = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
        bool isFirst = true;

        foreach (var part in orderParts)
        {
            var trimmedPart = part.Trim();
            var segments = trimmedPart.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var field = segments[0].ToLower();
            var direction = segments.Length > 1 ? segments[1].ToLower() : "asc";

            if (isFirst)
            {
                query = (field, direction) switch
                {
                    ("id", "desc") => query.OrderByDescending(c => c.Id),
                    ("id", _) => query.OrderBy(c => c.Id),
                    ("userid", "desc") => query.OrderByDescending(c => c.UserId),
                    ("userid", _) => query.OrderBy(c => c.UserId),
                    ("date", "desc") => query.OrderByDescending(c => c.Date),
                    ("date", _) => query.OrderBy(c => c.Date),
                    _ => query.OrderByDescending(c => c.Date)
                };
                isFirst = false;
            }
            else
            {
                var orderedQuery = (IOrderedQueryable<Cart>)query;
                query = (field, direction) switch
                {
                    ("id", "desc") => orderedQuery.ThenByDescending(c => c.Id),
                    ("id", _) => orderedQuery.ThenBy(c => c.Id),
                    ("userid", "desc") => orderedQuery.ThenByDescending(c => c.UserId),
                    ("userid", _) => orderedQuery.ThenBy(c => c.UserId),
                    ("date", "desc") => orderedQuery.ThenByDescending(c => c.Date),
                    ("date", _) => orderedQuery.ThenBy(c => c.Date),
                    _ => orderedQuery.ThenByDescending(c => c.Date)
                };
            }
        }

        return query;
    }
}

