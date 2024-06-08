using Common.Base;
using Common.DTO.UserDTO;

namespace MVC.Services.IService
{
    public interface IProfileService
    {
        Task<ResponseBase<UserDetailDTO?>> Index(string UserID);
        Task<ResponseBase<UserDetailDTO?>> Index(UserUpdateDTO DTO);
    }
}
