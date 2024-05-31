using DataAccess.DTO;

namespace MVC.Services.IService
{
    public interface ILogoutService
    {
        Task<ResponseDTO<bool>> Index(Guid UserID);
    }
}
