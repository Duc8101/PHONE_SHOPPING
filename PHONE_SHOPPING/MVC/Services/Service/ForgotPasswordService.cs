using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class ForgotPasswordService : BaseService, IForgotPasswordService
    {
        public ForgotPasswordService() : base()
        {
        }

        public async Task<ResponseDTO> ForgotPassword(ForgotPasswordDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/User/ForgotPassword";
                string requestData = getRequestData<ForgotPasswordDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
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
                return new ResponseDTO(false, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
