

using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using eCommerce.DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eCommerce.DataAccessLayer.Repositories;

public class ProductsRepository : IProductsRepository
{

    private readonly ApplicationDbContext _context;

    public ProductsRepository(ApplicationDbContext context)
    {
       _context = context;
    }

    public async Task<Product> AddProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProduct(Guid productID)
    {
        var existingProduct = await _context.Products.FirstOrDefaultAsync(temp => temp.ProductID == productID);
        if (existingProduct == null)
        {
            return false;
        }
        _context.Products.Remove(existingProduct);
        int affectedRowsCount = await _context.SaveChangesAsync();
        return affectedRowsCount > 0;
    }

    public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await _context.Products.FirstOrDefaultAsync<Product?>(conditionExpression);
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
       return await _context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await _context.Products.Where(conditionExpression).ToListAsync();
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        var existingProduct = await _context.Products.FirstOrDefaultAsync(temp => temp.ProductID == product.ProductID);
        if (existingProduct == null)
        {
            return null;
        }
        existingProduct.ProductName = product.ProductName;
        existingProduct.Category = product.Category;
        existingProduct.UnitPrice = product.UnitPrice;
        existingProduct.QuantityInStock = product.QuantityInStock;
        _context.Products.Update(existingProduct);
        await _context.SaveChangesAsync();
        return (existingProduct);
    }
}
