using Common.Base;
using Common.DTO.CartDTO;
using Common.DTO.OrderDetailDTO;
using Common.DTO.OrderDTO;
using Common.Pagination;

namespace API.Services.IService
{
    public interface IOrderService
    {
        Task<ResponseBase<List<CartListDTO>?>> Create(OrderCreateDTO DTO, Guid userId);
        Task<ResponseBase<Pagination<OrderListDTO>?>> List(Guid? UserID, string? status, bool isAdmin, int page);
        Task<ResponseBase<OrderDetailDTO?>> Detail(Guid OrderID);
        Task<ResponseBase<OrderDetailDTO?>> Update(Guid OrderID, OrderUpdateDTO DTO);
    }
}
