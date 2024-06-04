using DataAccess.Base;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.OrderDTO;

namespace MVC.Services.IService
{
    public interface ICartService
    {
        Task<ResponseBase<bool>> Create(CartCreateRemoveDTO DTO);
        Task<ResponseBase<List<CartListDTO>?>> Index();
        Task<ResponseBase<bool>> Remove(CartCreateRemoveDTO DTO);
        Task<ResponseBase<List<CartListDTO>?>> Checkout(OrderCreateDTO DTO);

    }
}
