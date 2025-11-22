using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace ProductServicve.API.APIEndpoints;

public static class APIEndpoints
{
    public static IEndpointRouteBuilder MapProductAPIEndpoints(this IEndpointRouteBuilder app)
    {
        var productGroup = app.MapGroup("/api/products").WithTags("Products");
        productGroup.MapGet("/", async (IProductService productService) =>
        {
            List<ProductResponse> products = await productService.GetProducts();
            return Results.Ok(products);
        });
        productGroup.MapGet("/search/product-id/{ProductID:guid}", async (IProductService productService, Guid ProductID) =>
        {
            ProductResponse? product = await productService.GetSingleProductByCondition(temp => temp.ProductID == ProductID);
            return Results.Ok(product);
        });
        productGroup.MapGet("/search/{searchString}", async (IProductService productService, string searchString) =>
        {
            var q = $"%{searchString.ToLowerInvariant()}%";
            List<ProductResponse?> productByProductName = await productService
                                              .GetProductsByCondition(temp =>
                                                                      temp.ProductName != null 
                                                                      && EF.Functions.Like(temp.ProductName.ToLower(), q));

            List<ProductResponse?> productByCategory = await productService
                                              .GetProductsByCondition(temp =>
                                                                      temp.Category != null
                                                                       && EF.Functions.Like(temp.Category.ToLower(), q));

            var products = productByProductName.Union(productByCategory);
            return Results.Ok(products);
        });
        productGroup.MapDelete("/{ProductID}", async (IProductService productService, Guid ProductID) =>
        {
            bool isProductDeleted = await productService.DeleteProduct(ProductID);
            if (!isProductDeleted)
            {
                return Results.Problem($"Product with ID {ProductID} not deleted.");
            }
            return Results.Ok(isProductDeleted);
        });
        productGroup.MapPost("/", async (IProductService productService, IValidator<ProductAddRequest> validator ,ProductAddRequest req) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(req);
            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                return Results.ValidationProblem(errors);
            }
            var created = await productService.AddProduct(req);
            if (created is not null) return Results.Created($"/api/products/search/product-id/{created.ProductID}", created);
            else return Results.Problem("Error adding product.");
        });
        productGroup.MapPut("/", async (IProductService productService, IValidator<ProductUpdateRequest> validator, ProductUpdateRequest req) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(req);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                return Results.ValidationProblem(errors);
            }
            var created = await productService.UpdateProduct(req);
            if (created is not null) return Results.Ok(created);
            else return Results.Problem("Error updating product.");
        });
        return app;
    }
}
