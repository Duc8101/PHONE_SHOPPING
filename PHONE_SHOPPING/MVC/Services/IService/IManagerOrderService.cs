using DataAccess.DTO;
using DataAccess.DTO.OrderDTO;

namespace MVC.Services.IService
{
    public interface IManagerOrderService
    {
        Task<ResponseDTO> Index(string? status, int? page);
        Task<ResponseDTO> View(Guid UserID);
        Task<ResponseDTO> Detail(Guid OrderID);
        Task<ResponseDTO> Update(Guid OrderID, OrderUpdateDTO DTO);
    }
}
