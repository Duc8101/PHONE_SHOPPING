using DataAccess.Base;
using DataAccess.DTO.UserDTO;

namespace MVC.Services.IService
{
    public interface IForgotPasswordService
    {
        Task<ResponseBase<bool>> ForgotPassword(ForgotPasswordDTO DTO);
    }
}
