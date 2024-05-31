using DataAccess.DTO.CartDTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.DTO;
using DataAccess.DTO.OrderDetailDTO;

namespace API.Services.IService
{
    public interface IOrderService
    {
        Task<ResponseDTO<List<CartListDTO>?>> Create(OrderCreateDTO DTO);
        Task<ResponseDTO<PagedResultDTO<OrderListDTO>?>> List(Guid? UserID, string? status, bool isAdmin, int page);
        Task<ResponseDTO<OrderDetailDTO?>> Detail(Guid OrderID);
        Task<ResponseDTO<OrderDetailDTO?>> Update(Guid OrderID, OrderUpdateDTO DTO);
    }
}
