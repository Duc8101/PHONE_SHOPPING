using DataAccess.DTO.CartDTO;
using DataAccess.DTO;
using DataAccess.DTO.OrderDTO;

namespace MVC.Services.IService
{
    public interface ICartService
    {
        Task<ResponseDTO<bool>> Create(CartCreateRemoveDTO DTO);
        Task<ResponseDTO<List<CartListDTO>?>> Index(string UserID);
        Task<ResponseDTO<bool>> Remove(CartCreateRemoveDTO DTO);
        Task<ResponseDTO<List<CartListDTO>?>> Checkout(OrderCreateDTO DTO);
    }
}
