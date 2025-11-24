using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IProductRepository using Entity Framework Core
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of ProductRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public ProductRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new product in the database
    /// </summary>
    /// <param name="product">The product to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created product</returns>
    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    /// <summary>
    /// Retrieves a product by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product if found, null otherwise</returns>
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves all products with optional filtering, pagination and ordering
    /// </summary>
    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetAllAsync(
        int page = 1,
        int size = 10,
        string? orderBy = null,
        Dictionary<string, string>? filters = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Products.AsQueryable();

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
            query = query.OrderBy(p => p.Title);
        }

        // Apply pagination
        var products = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (products, totalCount);
    }

    /// <summary>
    /// Retrieves all products by category with optional pagination and ordering
    /// </summary>
    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetByCategoryAsync(
        string category,
        int page = 1,
        int size = 10,
        string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Products
            .Where(p => p.Category.ToLower() == category.ToLower());

        var totalCount = await query.CountAsync(cancellationToken);

        // Apply ordering
        if (!string.IsNullOrEmpty(orderBy))
        {
            query = ApplyOrdering(query, orderBy);
        }
        else
        {
            query = query.OrderBy(p => p.Title);
        }

        var products = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (products, totalCount);
    }

    /// <summary>
    /// Retrieves all distinct product categories
    /// </summary>
    public async Task<IEnumerable<string>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="product">The product to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated product</returns>
    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    /// <summary>
    /// Deletes a product by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the product was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetByIdAsync(id, cancellationToken);
        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Applies filters to the query based on the provided dictionary
    /// </summary>
    private IQueryable<Product> ApplyFilters(IQueryable<Product> query, Dictionary<string, string> filters)
    {
        foreach (var filter in filters)
        {
            var key = filter.Key.ToLower();
            var value = filter.Value;

            switch (key)
            {
                case "category":
                    if (value.StartsWith("*"))
                        query = query.Where(p => p.Category.EndsWith(value.Substring(1)));
                    else if (value.EndsWith("*"))
                        query = query.Where(p => p.Category.StartsWith(value.Substring(0, value.Length - 1)));
                    else
                        query = query.Where(p => p.Category == value);
                    break;

                case "title":
                    if (value.StartsWith("*"))
                        query = query.Where(p => p.Title.EndsWith(value.Substring(1)));
                    else if (value.EndsWith("*"))
                        query = query.Where(p => p.Title.StartsWith(value.Substring(0, value.Length - 1)));
                    else
                        query = query.Where(p => p.Title == value);
                    break;

                case "_minprice":
                    if (decimal.TryParse(value, out var minPrice))
                        query = query.Where(p => p.Price >= minPrice);
                    break;

                case "_maxprice":
                    if (decimal.TryParse(value, out var maxPrice))
                        query = query.Where(p => p.Price <= maxPrice);
                    break;

                case "price":
                    if (decimal.TryParse(value, out var exactPrice))
                        query = query.Where(p => p.Price == exactPrice);
                    break;
            }
        }

        return query;
    }

    /// <summary>
    /// Applies ordering to the query based on the provided order string
    /// Format: "field1 asc, field2 desc" or "field1, field2 desc"
    /// </summary>
    private IQueryable<Product> ApplyOrdering(IQueryable<Product> query, string orderBy)
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
                    ("id", "desc") => query.OrderByDescending(p => p.Id),
                    ("id", _) => query.OrderBy(p => p.Id),
                    ("title", "desc") => query.OrderByDescending(p => p.Title),
                    ("title", _) => query.OrderBy(p => p.Title),
                    ("price", "desc") => query.OrderByDescending(p => p.Price),
                    ("price", _) => query.OrderBy(p => p.Price),
                    ("category", "desc") => query.OrderByDescending(p => p.Category),
                    ("category", _) => query.OrderBy(p => p.Category),
                    _ => query.OrderBy(p => p.Title)
                };
                isFirst = false;
            }
            else
            {
                var orderedQuery = (IOrderedQueryable<Product>)query;
                query = (field, direction) switch
                {
                    ("id", "desc") => orderedQuery.ThenByDescending(p => p.Id),
                    ("id", _) => orderedQuery.ThenBy(p => p.Id),
                    ("title", "desc") => orderedQuery.ThenByDescending(p => p.Title),
                    ("title", _) => orderedQuery.ThenBy(p => p.Title),
                    ("price", "desc") => orderedQuery.ThenByDescending(p => p.Price),
                    ("price", _) => orderedQuery.ThenBy(p => p.Price),
                    ("category", "desc") => orderedQuery.ThenByDescending(p => p.Category),
                    ("category", _) => orderedQuery.ThenBy(p => p.Category),
                    _ => orderedQuery.ThenBy(p => p.Title)
                };
            }
        }

        return query;
    }
}


