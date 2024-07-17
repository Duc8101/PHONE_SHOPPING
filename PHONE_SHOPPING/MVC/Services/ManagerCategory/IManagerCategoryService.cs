using Common.Base;
using Common.DTO.CategoryDTO;
using Common.Paginations;

namespace MVC.Services.ManagerCategory
{
    public interface IManagerCategoryService
    {
        Task<ResponseBase<Pagination<CategoryListDTO>?>> Index(string? name, int? page);
        Task<ResponseBase<bool?>> Create(CategoryCreateUpdateDTO DTO);
        Task<ResponseBase<CategoryListDTO?>> Update(int ID);
        Task<ResponseBase<CategoryListDTO?>> Update(int ID, CategoryCreateUpdateDTO DTO);
    }
}
