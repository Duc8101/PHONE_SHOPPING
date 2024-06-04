using DataAccess.Base;
using DataAccess.DTO.UserDTO;
using DataAccess.Entity;

namespace API.Services.IService
{
    public interface IUserService
    {
        Task<ResponseBase<UserDetailDTO?>> Detail(Guid userId);
        Task<ResponseBase<UserDetailDTO?>> Login(LoginDTO DTO);
        Task<ResponseBase<bool>> Create(UserCreateDTO DTO);
        Task<ResponseBase<bool>> ForgotPassword(ForgotPasswordDTO DTO);
        Task<ResponseBase<UserDetailDTO?>> Update(User user, UserUpdateDTO DTO);
        Task<ResponseBase<bool>> ChangePassword(User user, ChangePasswordDTO DTO);
        Task<ResponseBase<bool>> Logout(Guid UserID);
    }
}
