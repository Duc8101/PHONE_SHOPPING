using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using System.Net;

namespace MVC.Services
{
    public class LoginService : BaseService
    {

        public LoginService() : base()
        {

        }

        public async Task<ResponseDTO<UserDTO?>> Index(string UserID)
        {
            try
            {
                string URL = "https://localhost:7033/User/Detail/" + UserID;
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO<UserDTO?>? result = Deserialize<ResponseDTO<UserDTO?>>(data);
                if(result == null)
                {
                    return new ResponseDTO<UserDTO?>(null, data, (int) response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO<UserDTO?>(result.Data, string.Empty);
                }
                return new ResponseDTO<UserDTO?>(null, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDTO?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
