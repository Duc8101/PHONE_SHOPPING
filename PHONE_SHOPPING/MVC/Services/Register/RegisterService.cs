using Common.Base;
using Common.DTO.UserDTO;
using MVC.Services.Base;

namespace MVC.Services.Register
{
    public class RegisterService : BaseService, IRegisterService
    {
        public RegisterService(HttpClient client) : base(client)
        {

        }
        public async Task<ResponseBase<bool?>> Index(UserCreateDTO DTO)
        {
            string URL = "https://localhost:7178/User/Create";
            return await Post<UserCreateDTO, bool?>(URL, DTO);
        }
    }
}
