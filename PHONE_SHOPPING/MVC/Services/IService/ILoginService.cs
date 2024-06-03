using DataAccess.DTO.UserDTO;
using DataAccess.DTO;

namespace MVC.Services.IService
{
    public interface ILoginService
    {
        Task<ResponseDTO> Index(string UserID);
        Task<ResponseDTO> Index(LoginDTO DTO);
    }
}
