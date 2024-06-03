using DataAccess.DTO;

namespace MVC.Services.IService
{
    public interface IMyOrderService
    {
        Task<ResponseDTO> Index(string UserID, int? page);
        Task<ResponseDTO> Detail(Guid OrderID, Guid UserID);
    }
}
