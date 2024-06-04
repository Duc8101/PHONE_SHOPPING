using DataAccess.Base;
using DataAccess.DTO.UserDTO;

namespace MVC.Services.IService
{
    public interface IRegisterService
    {
        Task<ResponseBase<bool>> Index(UserCreateDTO DTO);
    }
}
