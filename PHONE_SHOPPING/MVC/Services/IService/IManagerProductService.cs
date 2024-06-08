using Common.Base;
using Common.DTO.CategoryDTO;
using Common.DTO.ProductDTO;

namespace MVC.Services.IService
{
    public interface IManagerProductService
    {
        Task<ResponseBase<Dictionary<string, object>?>> Index(string? name, int? CategoryID, int? page);
        Task<ResponseBase<List<CategoryListDTO>?>> Create();
        Task<ResponseBase<List<CategoryListDTO>?>> Create(ProductCreateUpdateDTO DTO);
        Task<ResponseBase<Dictionary<string, object>?>> Update(Guid ProductID);
        Task<ResponseBase<Dictionary<string, object>?>> Update(Guid ProductID, ProductCreateUpdateDTO DTO);
        Task<ResponseBase<Dictionary<string, object>?>> Delete(Guid ProductID);
    }
}
