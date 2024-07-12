using Common.Base;
using Common.DTO.UserDTO;
using MVC.Hardware;
using MVC.Services.Base;
using System.Net;

namespace MVC.Services.Login
{
    public class LoginService : BaseService, ILoginService
    {

        public async Task<ResponseBase<UserLoginInfoDTO?>> Index(string token)
        {
            string hardInfo = HardInfo.Generate();
            string URL = "https://localhost:7077/User/GetUserByToken";
            return await Get<UserLoginInfoDTO?>(URL, new KeyValuePair<string, object>("token", token)
                , new KeyValuePair<string, object>("hardware", hardInfo));
        }

        public async Task<ResponseBase<UserLoginInfoDTO?>> Index(LoginDTO DTO)
        {
            if (DTO.Password == null)
            {
                return new ResponseBase<UserLoginInfoDTO?>(null, "Username or password incorrect", (int)HttpStatusCode.Conflict);
            }
            string hardInfo = HardInfo.Generate();
            DTO.HarewareInfo = hardInfo;
            string URL = "https://localhost:7077/User/Login";
            return await Post<LoginDTO, UserLoginInfoDTO?>(URL, DTO);
        }
    }
}
