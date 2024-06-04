using DataAccess.DTO.UserDTO;
using DataAccess.DTO;
using System.Net;
using MVC.Services.IService;
using System.Text.Json;
using DataAccess.Const;
using System.Text;

namespace MVC.Services.Service
{
    public class ProfileService : BaseService, IProfileService
    {
        public ProfileService() : base()
        {
        }

        public async Task<ResponseDTO> Index(string UserID)
        {
            try
            {
                string URL = "https://localhost:7033/User/Detail/" + UserID;
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseDTO? result = Deserialize(data);
                if (result == null)
                {
                    return new ResponseDTO(null, data, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> Index(string UserID, UserUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/User/Update/" + UserID;
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PutAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseDTO? result = Deserialize(responseData);
                if (result == null)
                {
                    return new ResponseDTO(null, responseData, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
