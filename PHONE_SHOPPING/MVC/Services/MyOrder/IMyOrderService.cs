using Common.Base;
using Common.DTO.OrderDetailDTO;
using Common.DTO.OrderDTO;
using Common.Paginations;

namespace MVC.Services.MyOrder
{
    public interface IMyOrderService
    {
        Task<ResponseBase<Pagination<OrderListDTO>?>> Index(int? page);
        Task<ResponseBase<OrderDetailDTO?>> Detail(Guid OrderID, Guid UserID);
    }
}
