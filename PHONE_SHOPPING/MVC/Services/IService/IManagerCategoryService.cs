using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO;

namespace MVC.Services.IService
{
    public interface IManagerCategoryService
    {
        Task<ResponseDTO> Index(string? name, int? page);
        Task<ResponseDTO> Create(CategoryCreateUpdateDTO DTO);
        Task<ResponseDTO> Update(int ID);
        Task<ResponseDTO> Update(int ID, CategoryCreateUpdateDTO DTO);
    }
}
