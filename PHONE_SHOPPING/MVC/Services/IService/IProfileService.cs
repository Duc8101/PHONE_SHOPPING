using DataAccess.DTO.UserDTO;
using DataAccess.DTO;

namespace MVC.Services.IService
{
    public interface IProfileService
    {
        Task<ResponseDTO<UserDetailDTO?>> Index(string UserID);
        Task<ResponseDTO<UserDetailDTO?>> Index(string UserID, UserUpdateDTO DTO);
    }
}
