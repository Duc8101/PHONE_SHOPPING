using AutoMapper;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
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
            CreateMap<User, UserDetailDTO>()
                .ForMember(d => d.RoleName, m => m.MapFrom(source => source.Role.Name));
            CreateMap<Cart, CartListDTO>()
                .ForMember(d => d.ProductName, m => m.MapFrom(source => source.Product.ProductName))
                .ForMember(d => d.Image, m => m.MapFrom(source => source.Product.Image))
                .ForMember(d => d.Price, m => m.MapFrom(source => source.Product.Price));
            CreateMap<UserCreateDTO, User>()
                .ForMember(d => d.Email, m => m.MapFrom(source => source.Email.Trim()));
            CreateMap<OrderCreateDTO, Order>();
            CreateMap<Order, OrderListDTO>()
                .ForMember(d => d.Username, m => m.MapFrom(source => source.User.Username))
                .ForMember(d => d.OrderDate, m => m.MapFrom(source => source.CreatedAt));
            CreateMap<OrderDetail, DetailDTO>()
                .ForMember(d => d.ProductName, m => m.MapFrom(source => source.Product.ProductName))
                .ForMember(d => d.Image, m => m.MapFrom(source => source.Product.Image))
                .ForMember(d => d.Price, m => m.MapFrom(source => source.Product.Price))
                .ForMember(d => d.CategoryId, m => m.MapFrom(source => source.Product.CategoryId))
                .ForMember(d => d.CategoryName, m => m.MapFrom(source => source.Product.Category.Name));
            CreateMap<ProductCreateUpdateDTO, Product>()
                .ForMember(d => d.ProductName, m => m.MapFrom(source => source.ProductName.Trim()))
                .ForMember(d => d.Image, m => m.MapFrom(source => source.Image.Trim()));
        }
    }
}
