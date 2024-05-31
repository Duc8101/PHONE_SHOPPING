using DataAccess.DTO.ProductDTO;
using DataAccess.DTO;

namespace API.Services.IService
{
    public interface IProductService
    {
        Task<ResponseDTO<PagedResultDTO<ProductListDTO>?>> List(bool isAdmin, string? name, int? CategoryID, int page);
        Task<ResponseDTO<bool>> Create(ProductCreateUpdateDTO DTO);
        Task<ResponseDTO<ProductListDTO?>> Detail(Guid ProductID);
        Task<ResponseDTO<ProductListDTO?>> Update(Guid ProductID, ProductCreateUpdateDTO DTO);
        Task<ResponseDTO<PagedResultDTO<ProductListDTO>?>> Delete(Guid ProductID);
    }
}
