using DataAccess.DTO.UserDTO;
using DataAccess.DTO;

namespace MVC.Services.IService
{
    public interface IChangePasswordService
    {
        Task<ResponseDTO<bool>> Index(string UserID, ChangePasswordDTO DTO);
    }
}
