using DataAccess.DTO.UserDTO;
using DataAccess.DTO;

namespace MVC.Services.IService
{
    public interface IRegisterService
    {
        Task<ResponseDTO> Register(UserCreateDTO DTO);
    }
}
