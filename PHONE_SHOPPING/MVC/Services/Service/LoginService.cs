using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class LoginService : BaseService, ILoginService
    {
        public LoginService() : base()
        {
        }

        /*public async Task<ResponseDTO<UserDetailDTO?>> Index(string UserID)
        {
            try
            {
                string URL = "https://localhost:7033/User/Detail/" + UserID;
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO<UserDetailDTO?>? result = Deserialize<ResponseDTO<UserDetailDTO?>>(data);
                if (result == null)
                {
                    return new ResponseDTO<UserDetailDTO?>(null, data, (int)response.StatusCode);
                }
                return new ResponseDTO<UserDetailDTO?>(result.Data, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDetailDTO?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }*/

        public async Task<ResponseDTO<UserDetailDTO?>> Index(LoginDTO DTO)
        {
            try
            {
                if (DTO.Password == null)
                {
                    return new ResponseDTO<UserDetailDTO?>(null, "Username or password incorrect", (int)HttpStatusCode.Conflict);
                }
                string URL = "https://localhost:7033/User/Login";
                string requestData = getRequestData<LoginDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO<UserDetailDTO?>? result = Deserialize<ResponseDTO<UserDetailDTO?>>(responseData);
                if (result == null)
                {
                    return new ResponseDTO<UserDetailDTO?>(null, responseData, (int)response.StatusCode);
                }
                return new ResponseDTO<UserDetailDTO?>(result.Data, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDetailDTO?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
