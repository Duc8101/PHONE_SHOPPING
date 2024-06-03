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

        public async Task<ResponseDTO> Index(string UserID, ChangePasswordDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/User/ChangePassword/" + UserID;
                string requestData = getRequestData<ChangePasswordDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PutAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO? result = Deserialize<ResponseDTO>(responseData);
                if (result == null)
                {
                    return new ResponseDTO(false, responseData, (int)response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO(true, result.Message);
                }
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    return new ResponseDTO(result.Data, result.Message, (int)response.StatusCode);
                }
                return new ResponseDTO(false, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(false, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
