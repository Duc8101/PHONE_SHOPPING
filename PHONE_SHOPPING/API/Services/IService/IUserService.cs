using DataAccess.DTO.UserDTO;
using DataAccess.DTO;

namespace API.Services.IService
{
    public interface IUserService
    {
        Task<ResponseDTO> Detail(Guid UserID);
        Task<ResponseDTO> Login(LoginDTO DTO);
        Task<ResponseDTO> Create(UserCreateDTO DTO);
        Task<ResponseDTO> ForgotPassword(ForgotPasswordDTO DTO);
        Task<ResponseDTO> Update(Guid UserID, UserUpdateDTO DTO);
        Task<ResponseDTO> ChangePassword(Guid UserID, ChangePasswordDTO DTO);
        Task<ResponseDTO> Logout(Guid UserID);
    }
}
