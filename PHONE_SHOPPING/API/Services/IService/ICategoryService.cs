using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO;

namespace API.Services.IService
{
    public interface ICategoryService
    {
        Task<ResponseDTO<List<CategoryListDTO>?>> ListAll();
        Task<ResponseDTO<PagedResultDTO<CategoryListDTO>?>> ListPaged(string? name, int page);
        Task<ResponseDTO<bool>> Create(CategoryCreateUpdateDTO DTO);
        Task<ResponseDTO<CategoryListDTO?>> Detail(int ID);
        Task<ResponseDTO<CategoryListDTO?>> Update(int ID, CategoryCreateUpdateDTO DTO);
    }
}
