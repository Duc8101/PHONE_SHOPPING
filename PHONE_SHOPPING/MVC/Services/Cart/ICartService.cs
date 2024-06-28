using Common.Base;
using Common.DTO.CartDTO;
using Common.DTO.OrderDTO;

namespace MVC.Services.Cart
{
    public interface ICartService
    {
        Task<ResponseBase<bool>> Create(CartCreateDTO DTO);
        Task<ResponseBase<List<CartListDTO>?>> Index();
        Task<ResponseBase<bool>> Remove(Guid productId);
        Task<ResponseBase<List<CartListDTO>?>> Checkout(OrderCreateDTO DTO);

    }
}
