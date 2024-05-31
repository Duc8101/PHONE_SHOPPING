using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class ChangePasswordService : BaseService, IChangePasswordService
    {
        public ChangePasswordService() : base()
        {
        }

        public async Task<ResponseDTO<bool>> Index(string UserID, ChangePasswordDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/User/ChangePassword/" + UserID;
                string requestData = getRequestData<ChangePasswordDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PutAsync(URL, content);
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
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    return new ResponseDTO<bool>(result.Data, result.Message, (int)response.StatusCode);
                }
                return new ResponseDTO<bool>(false, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
