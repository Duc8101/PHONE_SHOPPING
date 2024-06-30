using Common.Base;
using Common.DTO.OrderDTO;

namespace API.Services.Orders
{
    public interface IOrderService
    {
        Task<ResponseBase> Create(OrderCreateDTO DTO, Guid userId);
        ResponseBase List(Guid? UserID, string? status, bool isAdmin, int page);
        ResponseBase Detail(Guid OrderID, Guid? userId);
        Task<ResponseBase> Update(Guid OrderID, OrderUpdateDTO DTO);
    }
}
