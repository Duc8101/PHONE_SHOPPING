using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using System.Net;

namespace MVC.Services
{
    public class LogoutService : BaseService
    {
        public async Task<ResponseDTO<bool>> Index(string UserID)
        {
            try
            {
                string URL = "https://localhost:7033/User/Logout?UserID=" + UserID;
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO<bool>? result = Deserialize<ResponseDTO<bool>>(data);
                if (result == null)
                {
                    return new ResponseDTO<bool>(false, data, (int) response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO<bool>(true, string.Empty);
                }
                return new ResponseDTO<bool>(false, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex) 
            {
                return new ResponseDTO<bool>(false, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
