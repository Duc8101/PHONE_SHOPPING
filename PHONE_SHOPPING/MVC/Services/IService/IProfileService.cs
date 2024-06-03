using DataAccess.DTO.UserDTO;
using DataAccess.DTO;

namespace MVC.Services.IService
{
    public interface IProfileService
    {
        Task<ResponseDTO> Index(string UserID);
        Task<ResponseDTO> Index(string UserID, UserUpdateDTO DTO);
    }
}
