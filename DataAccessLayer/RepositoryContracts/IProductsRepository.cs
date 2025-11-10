
using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace eCommerce.DataAccessLayer.RepositoryContracts;

public interface IProductsRepository
{
    /// <summary>
    /// The conditoom to get all products
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Product>> GetProducts();

    /// <summary>
    /// Get the products by condition
    /// </summary>
    /// <param name="conditionExpression"></param>
    /// <returns></returns>
    Task<IEnumerable<Product>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression);

    /// <summary>
    /// Gets single product by condition
    /// </summary>
    /// <param name="conditionExpression"></param>
    /// <returns></returns>
    Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression);

    /// <summary>
    /// Add a new product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task<Product> AddProduct(Product product);
    /// <summary>
    /// Update an existing product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task<Product?> UpdateProduct(Product product);
    /// <summary>
    /// Delete a product by ID
    /// </summary>
    /// <param name="productID"></param>
    /// <returns></returns>
    Task<bool> DeleteProduct(Guid productID);
}
