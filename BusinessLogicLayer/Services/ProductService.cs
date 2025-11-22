using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using eCommerce.DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Services;

public class ProductService : IProductService
{
    private readonly IValidator<ProductAddRequest> _productAddRequestValidator;
    private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
    private readonly IMapper _mapper;
    private readonly IProductsRepository _productsRepository;

    public ProductService(IValidator<ProductAddRequest> productAddRequestValidator,
                          IValidator<ProductUpdateRequest> productUpdateRequestValidator,
                          IMapper mapper,
                          IProductsRepository productRepository)
    {
        _mapper = mapper;
        _productsRepository = productRepository;
        _productAddRequestValidator = productAddRequestValidator;
        _productUpdateRequestValidator = productUpdateRequestValidator;
    }
    public async Task<ProductResponse> AddProduct(ProductAddRequest productAddRequest)
    {
        if(productAddRequest == null)
        {
            throw new ArgumentNullException(nameof(productAddRequest));
        }
        ValidationResult validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ArgumentException(errors);
        }
        Product? product = await _productsRepository.AddProduct(_mapper.Map<Product>(productAddRequest));
        if(product == null)
        {
            throw new InvalidOperationException("Product could not be added.");
        }
        return _mapper.Map<ProductResponse>(product);

    }

    public async Task<bool> DeleteProduct(Guid productID)
    {
        Product? product = await _productsRepository.GetProductByCondition(p => p.ProductID == productID);
        if(product == null)
        {
            return false;
        }
        bool isProductDeleted = await _productsRepository.DeleteProduct(productID);
        return  isProductDeleted;
    }

    public async Task<List<ProductResponse>> GetProducts()
    {
       IEnumerable<Product> products = await  _productsRepository.GetProducts();
       return _mapper.Map<List<ProductResponse>>(products).ToList();
    }

    public async Task<List<ProductResponse>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        IEnumerable<Product> products = await _productsRepository.GetProductsByCondition(conditionExpression);
        if (products == null || !products.Any())
        {
            return new List<ProductResponse>();
        }
        return _mapper.Map<List<ProductResponse>>(products).ToList();

    }

    public async Task<ProductResponse?> GetSingleProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        Product? product = await _productsRepository.GetProductByCondition(conditionExpression);
        if (product == null)
        {
            return null;
        }
        return _mapper.Map<ProductResponse>(product);
    }

    public async Task<ProductResponse> UpdateProduct(ProductUpdateRequest product)
    {
        Product? existingProduct = await _productsRepository.GetProductByCondition(p => p.ProductID == product.ProductID);
        if (existingProduct == null)
        {
            throw new ArgumentNullException(nameof(product));
        }
        ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(product);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ArgumentException(errors);
        }
        Product? updatedProduct = await _productsRepository.UpdateProduct(_mapper.Map<Product>(product));
        if (updatedProduct == null)
        {
            throw new InvalidOperationException("Product could not be updated.");
        }
        return _mapper.Map<ProductResponse>(updatedProduct);
    }
}
