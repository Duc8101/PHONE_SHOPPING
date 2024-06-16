using Common.Base;
using Common.DTO.UserDTO;
using Common.Entity;

namespace API.Services.IService
{
    public interface IUserService
    {
        ResponseBase<UserDetailDTO?> Detail(Guid userId);
        ResponseBase<UserDetailDTO?> Login(LoginDTO DTO);
        Task<ResponseBase<bool>> Create(UserCreateDTO DTO);
        Task<ResponseBase<bool>> ForgotPassword(ForgotPasswordDTO DTO);
        ResponseBase<UserDetailDTO?> Update(User user, UserUpdateDTO DTO);
        ResponseBase<bool> ChangePassword(User user, ChangePasswordDTO DTO);
        ResponseBase<bool> Logout(Guid UserID);
    }
}
