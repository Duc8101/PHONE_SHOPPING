using DataAccess.Base;
using DataAccess.DTO.UserDTO;

namespace MVC.Services.IService
{
    public interface ILoginService
    {
        Task<ResponseBase<UserDetailDTO?>> Index(string UserID);
        Task<ResponseBase<UserDetailDTO?>> Index(LoginDTO DTO);
    }
}
