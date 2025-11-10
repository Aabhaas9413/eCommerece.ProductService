namespace BusinessLogicLayer.DTO;

public record ProductAddRequest(string ProductName, CategoryOptions Categorys, double UnitPrice, 
                                int? QuantityInStock)
{
    public ProductAddRequest():this(default, default,default,default)
    {       
    }
}
