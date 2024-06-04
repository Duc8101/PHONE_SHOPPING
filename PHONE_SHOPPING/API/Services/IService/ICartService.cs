using DataAccess.DTO.CartDTO;
using DataAccess.DTO;

namespace API.Services.IService
{
    public interface ICartService
    {
        Task<ResponseDTO> List(Guid UserID);
        Task<ResponseDTO> Create(CartCreateRemoveDTO DTO);
        Task<ResponseDTO> Remove(CartCreateRemoveDTO DTO);
    }
}
