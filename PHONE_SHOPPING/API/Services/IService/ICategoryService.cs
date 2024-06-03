using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO;

namespace API.Services.IService
{
    public interface ICategoryService
    {
        Task<ResponseDTO> ListAll();
        Task<ResponseDTO> ListPaged(string? name, int page);
        Task<ResponseDTO> Create(CategoryCreateUpdateDTO DTO);
        Task<ResponseDTO> Detail(int ID);
        Task<ResponseDTO> Update(int ID, CategoryCreateUpdateDTO DTO);
    }
}
