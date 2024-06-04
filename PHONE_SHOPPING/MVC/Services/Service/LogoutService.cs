using DataAccess.DTO;
using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class LogoutService : BaseService, ILogoutService
    {
        public LogoutService() : base()
        {
        }

        public async Task<ResponseDTO> Index(Guid UserID)
        {
            try
            {
                string URL = "https://localhost:7033/User/Logout";
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseDTO? result = Deserialize(data);
                if (result == null)
                {
                    return new ResponseDTO(false, data, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO(false, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
