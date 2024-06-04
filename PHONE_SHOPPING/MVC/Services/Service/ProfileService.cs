using DataAccess.Base;
using DataAccess.Const;
using DataAccess.DTO.UserDTO;
using MVC.Services.IService;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MVC.Services.Service
{
    public class ProfileService : BaseService, IProfileService
    {
        public ProfileService() : base()
        {
        }

        public async Task<ResponseBase<UserDetailDTO?>> Index(string UserID)
        {
            try
            {
                string URL = "https://localhost:7178/User/Detail/" + UserID;
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<UserDetailDTO?>? result = Deserialize<UserDetailDTO?>(data);
                if (result == null)
                {
                    return new ResponseBase<UserDetailDTO?>(null, data, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<UserDetailDTO?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase<UserDetailDTO?>> Index(UserUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7178/User/Update";
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PutAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseBase<UserDetailDTO?>? result = Deserialize<UserDetailDTO?>(responseData);
                if (result == null)
                {
                    return new ResponseBase<UserDetailDTO?>(null, responseData, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<UserDetailDTO?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
