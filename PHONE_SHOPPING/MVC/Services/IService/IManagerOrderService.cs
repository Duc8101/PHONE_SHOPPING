using DataAccess.Base;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.DTO.UserDTO;

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
