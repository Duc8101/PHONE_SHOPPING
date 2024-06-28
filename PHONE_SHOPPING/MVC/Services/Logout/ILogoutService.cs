using Common.Base;

namespace MVC.Services.Logout
{
    public interface ILogoutService
    {
        Task<ResponseBase<bool>> Index();
    }
}
