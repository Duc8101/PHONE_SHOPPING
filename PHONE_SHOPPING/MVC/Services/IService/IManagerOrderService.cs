using DataAccess.DTO;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.DTO.UserDTO;

namespace MVC.Services.IService
{
    public interface IManagerOrderService
    {
        Task<ResponseDTO<Dictionary<string, object>?>> Index(string? status, int? page);
        Task<ResponseDTO<UserDetailDTO?>> View(Guid UserID);
        Task<ResponseDTO<OrderDetailDTO?>> Detail(Guid OrderID);
        Task<ResponseDTO<OrderDetailDTO?>> Update(Guid OrderID, OrderUpdateDTO DTO);
    }
}
