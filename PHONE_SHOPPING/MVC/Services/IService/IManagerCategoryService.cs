using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO;

namespace MVC.Services.IService
{
    public interface IManagerCategoryService
    {
        Task<ResponseDTO<PagedResultDTO<CategoryListDTO>?>> Index(string? name, int? page);
        Task<ResponseDTO<bool>> Create(CategoryCreateUpdateDTO DTO);
        Task<ResponseDTO<CategoryListDTO?>> Update(int ID);
        Task<ResponseDTO<CategoryListDTO?>> Update(int ID, CategoryCreateUpdateDTO DTO);
    }
}
