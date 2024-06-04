using DataAccess.Base;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.Pagination;

namespace API.Services.IService
{
    public interface IOrderService
    {
        Task<ResponseBase<List<CartListDTO>?>> Create(OrderCreateDTO DTO);
        Task<ResponseBase<Pagination<OrderListDTO>?>> List(Guid? UserID, string? status, bool isAdmin, int page);
        Task<ResponseBase<OrderDetailDTO?>> Detail(Guid OrderID);
        Task<ResponseBase<OrderDetailDTO?>> Update(Guid OrderID, OrderUpdateDTO DTO);
    }
}
