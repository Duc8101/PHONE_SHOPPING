using Common.Base;
using Common.DTO.UserDTO;
using MVC.Services.Base;

namespace MVC.Services.Profile
{
    public class ProfileService : BaseService, IProfileService
    {

        public async Task<ResponseBase<UserDetailDTO?>> Index()
        {
            string URL = "https://localhost:7077/User/Detail";
            return await Get<UserDetailDTO?>(URL);
        }

        public async Task<ResponseBase<UserDetailDTO?>> Index(UserUpdateDTO DTO)
        {
            string URL = "https://localhost:7077/User/Update";
            return await Put<UserUpdateDTO, UserDetailDTO?>(URL, DTO);
        }
    }
}
