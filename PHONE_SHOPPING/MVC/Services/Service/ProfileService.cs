using DataAccess.DTO.UserDTO;
using DataAccess.DTO;
using System.Net;
using MVC.Services.IService;

namespace MVC.Services.Service
{
    public class ProfileService : BaseService, IProfileService
    {
        public ProfileService() : base()
        {
        }

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

        public async Task<ResponseDTO<UserDetailDTO?>> Index(string UserID, UserUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/User/Update/" + UserID;
                string requestData = getRequestData<UserUpdateDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PutAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO<UserDetailDTO?>? result = Deserialize<ResponseDTO<UserDetailDTO?>>(responseData);
                if (result == null)
                {
                    return new ResponseDTO<UserDetailDTO?>(null, responseData, (int)response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO<UserDetailDTO?>(result.Data, result.Message);
                }
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    return new ResponseDTO<UserDetailDTO?>(result.Data, result.Message, (int)response.StatusCode);
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
