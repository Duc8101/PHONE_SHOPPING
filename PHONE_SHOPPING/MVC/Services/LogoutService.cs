using DataAccess.DTO;
using System.Net;

namespace MVC.Services
{
    public class LogoutService : BaseService
    {
        public LogoutService(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseDTO<bool>> Index(Guid UserID)
        {
            try
            {
                /* string URL = "https://localhost:7033/User/Logout?UserID=" + UserID;
                 HttpResponseMessage response = await GetAsync(URL);*/
                string URL = "https://localhost:7033/Cart/Delete/" + UserID;
                HttpResponseMessage response = await DeleteAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO<bool>? result = Deserialize<ResponseDTO<bool>>(data);
                if (result == null)
                {
                    return new ResponseDTO<bool>(false, data, (int)response.StatusCode);
                }
                return new ResponseDTO<bool>(result.Data, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
