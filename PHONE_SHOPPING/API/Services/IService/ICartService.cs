using Common.Base;
using Common.DTO.CartDTO;

namespace API.Services.IService
{
    public interface ICartService
    {
        ResponseBase<List<CartListDTO>?> List(Guid UserID);
        ResponseBase<bool> Create(CartCreateDTO DTO, Guid userId);
        ResponseBase<bool> Delete(Guid productId, Guid userId);
    }
}
