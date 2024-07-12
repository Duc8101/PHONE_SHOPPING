using Common.Base;
using Common.DTO.UserDTO;
using MVC.Services.Base;

namespace MVC.Services.ForgotPassword
{
    public class ForgotPasswordService : BaseService, IForgotPasswordService
    {

        public async Task<ResponseBase<bool?>> ForgotPassword(ForgotPasswordDTO DTO)
        {
            string URL = "https://localhost:7077/User/ForgotPassword";
            return await Post<ForgotPasswordDTO, bool?>(URL, DTO);
        }
    }
}
