using DataAccess.DTO.UserDTO;
using DataAccess.DTO;

namespace MVC.Services.IService
{
    public interface ILoginService
    {
        Task<ResponseDTO<UserDetailDTO?>> Index(LoginDTO DTO);
    }
}
