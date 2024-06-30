using Common.Base;
using Common.DTO.UserDTO;

namespace MVC.Services.Register
{
    public interface IRegisterService
    {
        Task<ResponseBase<bool?>> Index(UserCreateDTO DTO);
    }
}
