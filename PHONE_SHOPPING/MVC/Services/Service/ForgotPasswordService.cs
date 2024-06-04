using DataAccess.Const;
using DataAccess.DTO.UserDTO;
using MVC.Services.IService;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;
using System.Text.Json;
using System.Text;
using DataAccess.Base;

namespace MVC.Services.Service
{
    public class ForgotPasswordService : BaseService, IForgotPasswordService
    {
        public ForgotPasswordService() : base()
        {
        }

        public async Task<ResponseBase<bool>> ForgotPassword(ForgotPasswordDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7178/User/ForgotPassword";
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PostAsync(URL, content);
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
                return new ResponseBase<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
