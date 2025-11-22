using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Mappers;

public class ProductUpdateRequestToProductMappingProfile : Profile
{

    public ProductUpdateRequestToProductMappingProfile()
    {
        // DTO (update) -> Entity
        CreateMap<DTO.ProductUpdateRequest, DataAccessLayer.Entities.Product>()
            .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Categorys.ToString()))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => (double?)src.UnitPrice))
            .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock));
    }
}
