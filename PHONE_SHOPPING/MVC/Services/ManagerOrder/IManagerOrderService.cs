using Common.Base;
using Common.DTO.OrderDetailDTO;
using Common.DTO.OrderDTO;
using Common.DTO.UserDTO;

namespace MVC.Services.ManagerOrder
{
    public interface IManagerOrderService
    {
        Task<ResponseBase<Dictionary<string, object>?>> Index(string? status, int? page);
        Task<ResponseBase<UserDetailDTO?>> View(Guid userId);
        Task<ResponseBase<OrderDetailDTO?>> Detail(Guid orderId);
        Task<ResponseBase<OrderDetailDTO?>> Update(Guid orderId, OrderUpdateDTO DTO);
    }
}
