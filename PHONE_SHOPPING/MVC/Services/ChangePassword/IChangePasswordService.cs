using Common.Base;
using Common.DTO.UserDTO;

namespace MVC.Services.ChangePassword
{
    public interface IChangePasswordService
    {
        Task<ResponseBase<bool>> Index(ChangePasswordDTO DTO);
    }
}
