using Common.Base;
using Common.DTO.UserDTO;
using MVC.Services.Base;

namespace MVC.Services.ChangePassword
{
    public class ChangePasswordService : BaseService, IChangePasswordService
    {
        public ChangePasswordService(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseBase<bool?>> Index(ChangePasswordDTO DTO)
        {
            string URL = "https://localhost:7077/User/ChangePassword";
            return await Put<ChangePasswordDTO, bool?>(URL, DTO);
        }
    }
}
