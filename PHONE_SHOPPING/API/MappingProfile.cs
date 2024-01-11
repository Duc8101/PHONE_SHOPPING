using AutoMapper;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
using DataAccess.Entity;

namespace API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductListDTO>()
                .ForMember(d => d.CategoryName, m => m.MapFrom(source => source.Category.Name));
            CreateMap<Category, CategoryListDTO>();
        }
    }
}
