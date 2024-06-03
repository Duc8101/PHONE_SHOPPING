using DataAccess.DTO;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.OrderDTO;

namespace MVC.Services.IService
{
    public interface ICartService
    {
        Task<ResponseDTO> Create(CartCreateRemoveDTO DTO);
        Task<ResponseDTO> Index(string UserID);
        Task<ResponseDTO> Remove(CartCreateRemoveDTO DTO);
        Task<ResponseDTO> Checkout(OrderCreateDTO DTO);
    }
}
