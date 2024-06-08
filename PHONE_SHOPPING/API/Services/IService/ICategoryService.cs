using Common.Base;
using Common.DTO.CategoryDTO;
using Common.Pagination;

namespace API.Services.IService
{
    public interface ICategoryService
    {
        Task<ResponseBase<List<CategoryListDTO>?>> ListAll();
        Task<ResponseBase<Pagination<CategoryListDTO>?>> ListPaged(string? name, int page);
        Task<ResponseBase<bool>> Create(CategoryCreateUpdateDTO DTO);
        Task<ResponseBase<CategoryListDTO?>> Detail(int ID);
        Task<ResponseBase<CategoryListDTO?>> Update(int ID, CategoryCreateUpdateDTO DTO);
    }
}
