using DataAccess.Base;

namespace MVC.Services.IService
{
    public interface ILogoutService
    {
        Task<ResponseBase<bool>> Index();
    }
}
