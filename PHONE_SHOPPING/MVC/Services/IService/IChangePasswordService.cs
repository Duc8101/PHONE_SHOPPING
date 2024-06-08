using Common.Base;
using Common.DTO.UserDTO;

namespace MVC.Services.IService
{
    public interface IChangePasswordService
    {
        Task<ResponseBase<bool>> Index(ChangePasswordDTO DTO);
    }
}
