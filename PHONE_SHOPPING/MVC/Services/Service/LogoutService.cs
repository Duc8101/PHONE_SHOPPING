using Common.Base;
using MVC.Services.IService;

namespace MVC.Services.Service
{
    public class LogoutService : BaseService, ILogoutService
    {
        public LogoutService(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseBase<bool>> Index()
        {
            string URL = "https://localhost:7178/User/Logout";
            return await Get<bool>(URL);
        }
    }
}
