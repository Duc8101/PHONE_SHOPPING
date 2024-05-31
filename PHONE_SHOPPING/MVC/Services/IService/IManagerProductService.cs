using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;

namespace MVC.Services.IService
{
    public interface IManagerProductService
    {
        Task<ResponseDTO<Dictionary<string, object>?>> Index(string? name, int? CategoryID, int? page);
        Task<ResponseDTO<List<CategoryListDTO>?>> Create();
        Task<ResponseDTO<List<CategoryListDTO>?>> Create(ProductCreateUpdateDTO DTO);
        Task<ResponseDTO<Dictionary<string, object>?>> Update(Guid ProductID);
        Task<ResponseDTO<Dictionary<string, object>?>> Update(Guid ProductID, ProductCreateUpdateDTO DTO);
        Task<ResponseDTO<Dictionary<string, object>?>> Delete(Guid ProductID);
    }
}
