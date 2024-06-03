using DataAccess.DTO;
using DataAccess.DTO.OrderDTO;

namespace API.Services.IService
{
    public interface IOrderService
    {
        Task<ResponseDTO> Create(OrderCreateDTO DTO);
        Task<ResponseDTO> List(Guid? UserID, string? status, bool isAdmin, int page);
        Task<ResponseDTO> Detail(Guid OrderID);
        Task<ResponseDTO> Update(Guid OrderID, OrderUpdateDTO DTO);
    }
}
