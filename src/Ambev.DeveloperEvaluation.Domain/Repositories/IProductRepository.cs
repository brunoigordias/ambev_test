using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Product entity operations
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Creates a new product in the repository
    /// </summary>
    /// <param name="product">The product to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created product</returns>
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a product by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product if found, null otherwise</returns>
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all products with optional filtering, pagination and ordering
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="size">Page size</param>
    /// <param name="orderBy">Order by expression</param>
    /// <param name="filters">Dictionary of filters to apply</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of products and total count</returns>
    Task<(IEnumerable<Product> Products, int TotalCount)> GetAllAsync(
        int page = 1,
        int size = 10,
        string? orderBy = null,
        Dictionary<string, string>? filters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all products by category with optional pagination and ordering
    /// </summary>
    /// <param name="category">The category to filter by</param>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="size">Page size</param>
    /// <param name="orderBy">Order by expression</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of products and total count</returns>
    Task<(IEnumerable<Product> Products, int TotalCount)> GetByCategoryAsync(
        string category,
        int page = 1,
        int size = 10,
        string? orderBy = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all distinct product categories
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of categories</returns>
    Task<IEnumerable<string>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="product">The product to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated product</returns>
    Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a product by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the product was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}


