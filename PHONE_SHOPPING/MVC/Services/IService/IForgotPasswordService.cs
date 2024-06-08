using Common.Base;
using Common.DTO.UserDTO;

namespace MVC.Services.IService
{
    public interface IForgotPasswordService
    {
        Task<ResponseBase<bool>> ForgotPassword(ForgotPasswordDTO DTO);
    }
}
