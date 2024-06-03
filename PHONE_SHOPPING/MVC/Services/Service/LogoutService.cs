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
                /* string URL = "https://localhost:7033/User/Logout?UserID=" + UserID;
                 HttpResponseMessage response = await GetAsync(URL);*/
                string URL = "https://localhost:7033/Cart/Delete/" + UserID;
                HttpResponseMessage response = await DeleteAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO? result = Deserialize<ResponseDTO>(data);
                if (result == null)
                {
                    return new ResponseDTO(false, data, (int)response.StatusCode);
                }
                return new ResponseDTO(result.Data, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(false, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
