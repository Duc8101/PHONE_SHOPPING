using DataAccess.DTO.UserDTO;
using DataAccess.DTO;
using System.Net;

namespace MVC.Services
{
    public class ProfileService : BaseService
    {
        public async Task<ResponseDTO<UserDetailDTO?>> Index(string UserID)
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
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO<UserDetailDTO?>(result.Data, string.Empty);
                }
                return new ResponseDTO<UserDetailDTO?>(null, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDetailDTO?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
