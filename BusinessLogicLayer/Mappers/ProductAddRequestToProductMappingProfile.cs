using AutoMapper;
using DataAccessLayer.Entities;
using BusinessLogicLayer.DTO;
using System;

namespace BusinessLogicLayer.Mappers;

public class ProductAddRequestToProductMappingProfile : Profile
{
    public ProductAddRequestToProductMappingProfile()
    {
        // DTO (create) -> Entity
        CreateMap<ProductAddRequest, Product>()
            .ForMember(dest => dest.ProductID, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Categorys.ToString()))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => (double?)src.UnitPrice))
            .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock));
    }
}
