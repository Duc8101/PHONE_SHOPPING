using Common.Base;
using Common.DTO.UserDTO;

namespace MVC.Services.Login
{
    public interface ILoginService
    {
        Task<ResponseBase<UserLoginInfoDTO?>> Index(string token);
        Task<ResponseBase<UserLoginInfoDTO?>> Index(LoginDTO DTO);
    }
}
