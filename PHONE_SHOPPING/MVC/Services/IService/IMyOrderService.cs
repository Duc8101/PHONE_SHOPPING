using DataAccess.Base;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.Pagination;

namespace MVC.Services.IService
{
    public interface IMyOrderService
    {
        Task<ResponseBase<Pagination<OrderListDTO>?>> Index(int? page);
        Task<ResponseBase<OrderDetailDTO?>> Detail(Guid OrderID, Guid UserID);
    }
}
