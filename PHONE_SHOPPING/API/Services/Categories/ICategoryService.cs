using Common.Base;
using Common.DTO.CategoryDTO;

namespace API.Services.Categories
{
    public interface ICategoryService
    {
        ResponseBase ListAll();
        ResponseBase ListPaged(string? name, int page);
        ResponseBase Create(CategoryCreateUpdateDTO DTO);
        ResponseBase Detail(int ID);
        ResponseBase Update(int ID, CategoryCreateUpdateDTO DTO);
    }
}
