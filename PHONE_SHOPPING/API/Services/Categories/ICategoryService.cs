using Common.Base;
using Common.DTO.CategoryDTO;
using Common.Pagination;

namespace API.Services.Categories
{
    public interface ICategoryService
    {
        ResponseBase<List<CategoryListDTO>?> ListAll();
        ResponseBase<Pagination<CategoryListDTO>?> ListPaged(string? name, int page);
        ResponseBase<bool> Create(CategoryCreateUpdateDTO DTO);
        ResponseBase<CategoryListDTO?> Detail(int ID);
        ResponseBase<CategoryListDTO?> Update(int ID, CategoryCreateUpdateDTO DTO);
    }
}
