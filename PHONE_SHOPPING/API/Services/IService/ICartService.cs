using Common.Base;
using Common.DTO.CartDTO;

namespace API.Services.IService
{
    public interface ICartService
    {
        Task<ResponseBase<List<CartListDTO>?>> List(Guid UserID);
        Task<ResponseBase<bool>> Create(CartCreateDTO DTO, Guid userId);
        Task<ResponseBase<bool>> Delete(Guid productId, Guid userId);
    }
}
