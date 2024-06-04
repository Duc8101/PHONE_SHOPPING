using DataAccess.Base;
using DataAccess.DTO.ProductDTO;
using DataAccess.Pagination;

namespace API.Services.IService
{
    public interface IProductService
    {
        Task<ResponseBase<Pagination<ProductListDTO>?>> List(bool isAdmin, string? name, int? CategoryID, int page);
        Task<ResponseBase<bool>> Create(ProductCreateUpdateDTO DTO);
        Task<ResponseBase<ProductListDTO?>> Detail(Guid ProductID);
        Task<ResponseBase<ProductListDTO?>> Update(Guid ProductID, ProductCreateUpdateDTO DTO);
        Task<ResponseBase<Pagination<ProductListDTO>?>> Delete(Guid ProductID);
    }
}
