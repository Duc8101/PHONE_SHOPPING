using DataAccess.DTO.CartDTO;
using DataAccess.DTO;

namespace API.Services.IService
{
    public interface ICartService
    {
        Task<ResponseDTO<List<CartListDTO>?>> List(Guid UserID);
        Task<ResponseDTO<bool>> Create(CartCreateRemoveDTO DTO);
        Task<ResponseDTO<bool>> Remove(CartCreateRemoveDTO DTO);
        Task<ResponseDTO<bool>> Delete(Guid UserID);
    }
}
