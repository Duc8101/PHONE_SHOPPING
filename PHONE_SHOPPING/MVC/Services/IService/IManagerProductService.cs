using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;

namespace MVC.Services.IService
{
    public interface IManagerProductService
    {
        Task<ResponseDTO> Index(string? name, int? CategoryID, int? page);
        Task<ResponseDTO> Create();
        Task<ResponseDTO> Create(ProductCreateUpdateDTO DTO);
        Task<ResponseDTO> Update(Guid ProductID);
        Task<ResponseDTO> Update(Guid ProductID, ProductCreateUpdateDTO DTO);
        Task<ResponseDTO> Delete(Guid ProductID);
    }
}
