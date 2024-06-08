using Common.Base;
using Common.DTO.ProductDTO;
using Common.Pagination;

namespace API.Services.IService
{
    public interface IProductService
    {
        Task<ResponseBase<Pagination<ProductListDTO>?>> List(bool isAdmin, string? name, int? CategoryID, int page);
        Task<ResponseBase<bool>> Create(ProductCreateUpdateDTO DTO);
        Task<ResponseBase<ProductListDTO?>> Detail(Guid ProductID);
        Task<ResponseBase<ProductListDTO?>> Update(Guid ProductID, ProductCreateUpdateDTO DTO);
        Task<ResponseBase<bool>> Delete(Guid ProductID);
    }
}
