using DataAccess.Base;
using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class LogoutService : BaseService, ILogoutService
    {
        public LogoutService() : base()
        {
        }

        public async Task<ResponseBase<bool>> Index()
        {
            try
            {
                string URL = "https://localhost:7178/User/Logout";
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<bool>? result = Deserialize<bool>(data);
                if (result == null)
                {
                    return new ResponseBase<bool>(false, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>(false, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
