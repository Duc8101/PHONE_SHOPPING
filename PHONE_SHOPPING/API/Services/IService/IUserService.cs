using DataAccess.DTO.UserDTO;
using DataAccess.DTO;

namespace API.Services.IService
{
    public interface IUserService
    {
        Task<ResponseDTO<UserDetailDTO?>> Detail(Guid UserID);
        Task<ResponseDTO<UserDetailDTO?>> Login(LoginDTO DTO);
        Task<ResponseDTO<bool>> Create(UserCreateDTO DTO);
        Task<ResponseDTO<bool>> ForgotPassword(ForgotPasswordDTO DTO);
        Task<ResponseDTO<UserDetailDTO?>> Update(Guid UserID, UserUpdateDTO DTO);
        Task<ResponseDTO<bool>> ChangePassword(Guid UserID, ChangePasswordDTO DTO);
    }
}
