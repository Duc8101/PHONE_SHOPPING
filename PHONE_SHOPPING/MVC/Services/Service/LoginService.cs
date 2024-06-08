using Common.Base;
using Common.DTO.UserDTO;
using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class LoginService : BaseService, ILoginService
    {
        public LoginService(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseBase<UserDetailDTO?>> Index(string UserID)
        {
            string URL = "https://localhost:7178/User/Detail/" + UserID;
            return await Get<UserDetailDTO?>(URL);
        }

        public async Task<ResponseBase<UserDetailDTO?>> Index(LoginDTO DTO)
        {
            if (DTO.Password == null)
            {
                return new ResponseBase<UserDetailDTO?>(null, "Username or password incorrect", (int)HttpStatusCode.Conflict);
            }
            string URL = "https://localhost:7178/User/Login";
            return await Post<LoginDTO, UserDetailDTO?>(URL, DTO);
        }
    }
}
