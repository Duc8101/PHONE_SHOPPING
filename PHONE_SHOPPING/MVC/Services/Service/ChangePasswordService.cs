using DataAccess.Base;
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

        public async Task<ResponseBase<bool>> Index(ChangePasswordDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7178/User/ChangePassword";
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PutAsync(URL, content); ;
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseBase<bool>? result = Deserialize<bool>(responseData);
                if (result == null)
                {
                    return new ResponseBase<bool>(false, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>(false, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
