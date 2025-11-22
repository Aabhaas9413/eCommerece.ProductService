using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Mappers;

public class MapProductToProductResponseMappingProfile : Profile
{
    public MapProductToProductResponseMappingProfile() 
    {
        // Entity -> DTO (response)
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Categorys, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.HasValue ? (decimal?)Convert.ToDecimal(src.UnitPrice.Value) : null))
            .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock));
    }
}
