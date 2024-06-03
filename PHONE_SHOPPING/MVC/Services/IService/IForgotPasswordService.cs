using DataAccess.DTO.UserDTO;
using DataAccess.DTO;

namespace MVC.Services.IService
{
    public interface IForgotPasswordService
    {
        Task<ResponseDTO> ForgotPassword(ForgotPasswordDTO DTO);
    }
}
