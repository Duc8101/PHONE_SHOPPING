using Common.Base;
using Common.DTO.OrderDetailDTO;
using Common.DTO.OrderDTO;
using Common.DTO.UserDTO;

namespace MVC.Services.IService
{
    public interface IManagerOrderService
    {
        Task<ResponseBase<Dictionary<string, object>?>> Index(string? status, int? page);
        Task<ResponseBase<UserDetailDTO?>> View(Guid UserID);
        Task<ResponseBase<OrderDetailDTO?>> Detail(Guid OrderID);
        Task<ResponseBase<OrderDetailDTO?>> Update(Guid OrderID, OrderUpdateDTO DTO);
    }
}
