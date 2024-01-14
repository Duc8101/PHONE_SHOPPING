using AutoMapper;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
using DataAccess.DTO.UserDTO;
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
            CreateMap<User, UserListDTO>()
                .ForMember(d => d.RoleName, m => m.MapFrom(source => source.Role.Name));
            CreateMap<Cart, CartListDTO>()
                .ForMember(d => d.ProductName, m => m.MapFrom(source => source.Product.ProductName))
                .ForMember(d => d.Image, m => m.MapFrom(source => source.Product.Image))
                .ForMember(d => d.Price, m => m.MapFrom(source => source.Product.Price));
            CreateMap<UserCreateDTO, User>()
                .ForMember(d => d.Email, m => m.MapFrom(source => source.Email.Trim()));
        }
    }
}
