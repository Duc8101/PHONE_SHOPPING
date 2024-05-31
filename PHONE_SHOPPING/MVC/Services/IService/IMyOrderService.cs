using DataAccess.DTO.OrderDTO;
using DataAccess.DTO;
using DataAccess.DTO.OrderDetailDTO;

namespace MVC.Services.IService
{
    public interface IMyOrderService
    {
        Task<ResponseDTO<PagedResultDTO<OrderListDTO>?>> Index(string UserID, int? page);
        Task<ResponseDTO<OrderDetailDTO?>> Detail(Guid OrderID, Guid UserID);
    }
}
