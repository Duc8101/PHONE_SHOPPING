using DataAccess.Base;
using DataAccess.DTO.UserDTO;
using MVC.Services.IService;

namespace MVC.Services.Service
{
    public class ForgotPasswordService : BaseService, IForgotPasswordService
    {
        public ForgotPasswordService(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseBase<bool>> ForgotPassword(ForgotPasswordDTO DTO)
        {
            string URL = "https://localhost:7178/User/ForgotPassword";
            return await Post<ForgotPasswordDTO, bool>(URL, DTO);
        }
    }
}
