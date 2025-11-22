using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace BusinessLogicLayer.ServiceContracts;

/// <summary>
/// Defines product-related operations exposed by the business logic layer.
/// Implementations handle retrieval, creation, update and deletion of products
/// and return DTOs appropriate for API responses.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Returns a list of products. 
    /// </summary>
    /// <returns></returns>
    Task<List<ProductResponse>> GetProducts();

    /// <summary>
    /// Retrieves products that satisfy the provided condition expression.
    /// Useful for custom queries such as filtering by category, price range, etc.
    /// </summary>
    /// <param name="conditionExpression">Expression used to filter products.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="ProductResponse"/> that match the condition.</returns>
     Task<List<ProductResponse>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression);

    /// <summary>
    /// Retrieves a single product (or an empty/nullable result) that satisfies the provided condition.
    /// The list wrapper is retained to match the current service contract; implementations should return
    /// at most one item in the list when used as a single-product lookup.
    /// </summary>
    /// <param name="conditionExpression">Expression used to locate the product.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list where the element
    /// may be <c>null</c> if no matching product was found; implementations typically return a single-item list.
    /// </returns>
    Task<ProductResponse?> GetSingleProductByCondition(Expression<Func<Product, bool>> conditionExpression);

    /// <summary>
    /// Adds a new product using values from the provided <see cref="ProductAddRequest"/>.
    /// </summary>
    /// <param name="product">The DTO containing product creation data.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created <see cref="ProductResponse"/>.
    /// </returns>
    Task<ProductResponse> AddProduct(ProductAddRequest product);

    /// <summary>
    /// Updates an existing product using values from the provided <see cref="ProductUpdateRequest"/>.
    /// </summary>
    /// <param name="product">The DTO containing product update data (including the product identifier).</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated <see cref="ProductResponse"/>.
    /// </returns>
    Task<ProductResponse> UpdateProduct(ProductUpdateRequest product);

    /// <summary>
    /// Deletes a product by its identifier.
    /// </summary>
    /// <param name="productID">The identifier of the product to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is <c>true</c> if the product was deleted;
    /// otherwise <c>false</c>.
    /// </returns>
    Task<bool> DeleteProduct(Guid productID);
}
