using DataAccess.Base;
using DataAccess.DTO.UserDTO;

namespace MVC.Services.IService
{
    public interface IChangePasswordService
    {
        Task<ResponseBase<bool>> Index(ChangePasswordDTO DTO);
    }
}
