using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using MVC.Services.IService;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MVC.Services.Service
{
    public class ChangePasswordService : BaseService, IChangePasswordService
    {
        public ChangePasswordService() : base()
        {
        }

        public async Task<ResponseDTO> Index(string UserID, ChangePasswordDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/User/ChangePassword/" + UserID;
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PutAsync(URL, content); ;
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
