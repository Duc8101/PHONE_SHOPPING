using DataAccess.Base;
using DataAccess.DTO.CartDTO;

namespace API.Services.IService
{
    public interface ICartService
    {
        Task<ResponseBase<List<CartListDTO>?>> List(Guid UserID);
        Task<ResponseBase<bool>> Create(CartCreateRemoveDTO DTO, Guid userId);
        Task<ResponseBase<bool>> Remove(CartCreateRemoveDTO DTO, Guid userId);
    }
}
