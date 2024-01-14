using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using System.Net;

namespace MVC.Services
{
    public class RegisterService : BaseService
    {
        public async Task<ResponseDTO<bool>> Register(UserCreateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/User/Create";
                string requestData = getRequestData<UserCreateDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO<bool>? result = Deserialize<ResponseDTO<bool>>(responseData);
                if (result == null)
                {
                    return new ResponseDTO<bool>(false, responseData, (int)response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO<bool>(true, result.Message);
                }
                return new ResponseDTO<bool>(false, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
