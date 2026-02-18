using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

/// <summary>
/// Repository interface for Product entity operations.
/// </summary>
public interface IProductRepository : IBaseRepository<Product>
{
    /// <summary>
    /// Check if GTIN is unique
    /// </summary>
    Task<bool> IsGTINUniqueAsync(string gtin, Guid? excludeId = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get product by GTIN
    /// </summary>
    Task<Product?> GetByGTINAsync(string gtin, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get products by type
    /// </summary>
    Task<IEnumerable<Product>> GetByTypeAsync(Guid productTypeId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get product with stock information
    /// </summary>
    Task<Product?> GetWithStockAsync(Guid productId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get product with batches
    /// </summary>
    Task<Product?> GetWithBatchesAsync(Guid productId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Search products by name or generic name
    /// </summary>
    Task<IEnumerable<Product>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get products with low stock
    /// </summary>
    Task<IEnumerable<Product>> GetLowStockProductsAsync(Guid branchId, CancellationToken cancellationToken = default);
}
