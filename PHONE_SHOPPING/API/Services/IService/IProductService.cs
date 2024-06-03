using DataAccess.DTO.ProductDTO;
using DataAccess.DTO;

namespace API.Services.IService
{
    public interface IProductService
    {
        Task<ResponseDTO> List(bool isAdmin, string? name, int? CategoryID, int page);
        Task<ResponseDTO> Create(ProductCreateUpdateDTO DTO);
        Task<ResponseDTO> Detail(Guid ProductID);
        Task<ResponseDTO> Update(Guid ProductID, ProductCreateUpdateDTO DTO);
        Task<ResponseDTO> Delete(Guid ProductID);
    }
}
