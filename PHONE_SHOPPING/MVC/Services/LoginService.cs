using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using DataAccess.Entity;
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

        public async Task<ResponseDTO<UserDTO?>> Index(LoginDTO DTO)
        {
            try
            {
                if (DTO.Password == null)
                {
                    return new ResponseDTO<UserDTO?>(null, "Username or password incorrect", (int)HttpStatusCode.Conflict);
                }
                string URL = "https://localhost:7033/User/Login";
                string requestData = getRequestData<LoginDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
                string data = await getResponseData(response);
                ResponseDTO<UserDTO?>? result = Deserialize<ResponseDTO<UserDTO?>>(data);
                if (result == null)
                {
                    return new ResponseDTO<UserDTO?>(null, data, (int) response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO<UserDTO?>(result.Data, string.Empty);
                }
                return new ResponseDTO<UserDTO?>(null, result.Message, (int) response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDTO?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
