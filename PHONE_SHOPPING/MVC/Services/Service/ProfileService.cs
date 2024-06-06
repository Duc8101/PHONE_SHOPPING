using DataAccess.Base;
using DataAccess.DTO.UserDTO;
using MVC.Services.IService;

namespace MVC.Services.Service
{
    public class ProfileService : BaseService, IProfileService
    {
        public ProfileService(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseBase<UserDetailDTO?>> Index(string UserID)
        {
            string URL = "https://localhost:7178/User/Detail/" + UserID;
            return await Get<UserDetailDTO?>(URL);
        }

        public async Task<ResponseBase<UserDetailDTO?>> Index(UserUpdateDTO DTO)
        {
            string URL = "https://localhost:7178/User/Update";
            return await Put<UserUpdateDTO, UserDetailDTO?>(URL, DTO);
        }
    }
}
