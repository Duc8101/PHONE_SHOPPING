using AutoMapper;
using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using DataAccess.Entity;
using DataAccess.Model;
using DataAccess.Model.DAO;
using System.Net;
using System.Text.RegularExpressions;

namespace API.Services
{
    public class UserService : BaseService
    {
        private readonly DAOUser daoUser = new DAOUser();
        private readonly DAOCart daoCart = new DAOCart();
        public UserService(IMapper mapper) : base(mapper)
        {

        }
        public async Task<ResponseDTO<UserDetailDTO?>> Detail(Guid UserID)
        {
            try
            {
                User? user = await daoUser.getUser(UserID);
                if (user == null)
                {
                    return new ResponseDTO<UserDetailDTO?>(null, "Not found user", (int)HttpStatusCode.NotFound);
                }
                UserDetailDTO DTO = mapper.Map<UserDetailDTO>(user);
                return new ResponseDTO<UserDetailDTO?>(DTO, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<UserDetailDTO?>> Login(LoginDTO DTO)
        {
            try
            {
                User? user = await daoUser.getUser(DTO);
                if (user == null)
                {
                    return new ResponseDTO<UserDetailDTO?>(null, "Username or password incorrect", (int)HttpStatusCode.NotFound);
                }
                UserDetailDTO data = mapper.Map<UserDetailDTO>(user);
                return new ResponseDTO<UserDetailDTO?>(data, string.Empty);

            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        /*public async Task<ResponseDTO<bool>> Logout(Guid UserID)
        {
            try
            {
                List<Cart> list = await daoCart.getList(UserID);
                foreach (Cart cart in list)
                {
                    await daoCart.DeleteCart(cart);
                }
                return new ResponseDTO<bool>(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }*/

        public async Task<ResponseDTO<bool>> Create(UserCreateDTO DTO)
        {
            try
            {
                Regex regex = new Regex(UserConst.FORMAT_EMAIL);
                if (!regex.IsMatch(DTO.Email.Trim()))
                {
                    return new ResponseDTO<bool>(false, "Invalid email", (int)HttpStatusCode.Conflict);
                }
                if (await daoUser.isExist(DTO.Username, DTO.Email.Trim()))
                {
                    return new ResponseDTO<bool>(false, "Username or email has existed", (int)HttpStatusCode.Conflict);
                }
                string newPw = UserUtil.RandomPassword();
                string hashPw = UserUtil.HashPassword(newPw);
                // get body email
                string body = UserUtil.BodyEmailForRegister(newPw);
                // send email
                await UserUtil.sendEmail("Welcome to PHONE SHOPPING", body, DTO.Email.Trim());
                User user = mapper.Map<User>(DTO);
                user.UserId = Guid.NewGuid();
                user.Password = hashPw;
                user.RoleId = RoleConst.ROLE_CUSTOMER;
                user.CreatedAt = DateTime.Now;
                user.UpdateAt = DateTime.Now;
                user.IsDeleted = false;
                await daoUser.CreateUser(user);
                return new ResponseDTO<bool>(true, "Register successful");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<bool>> ForgotPassword(ForgotPasswordDTO DTO)
        {
            try
            {
                User? user = await daoUser.getUser(DTO.Email.Trim());
                if (user == null)
                {
                    return new ResponseDTO<bool>(false, "Not found email", (int)HttpStatusCode.NotFound);
                }
                string newPw = UserUtil.RandomPassword();
                string hashPw = UserUtil.HashPassword(newPw);
                // get body email
                string body = UserUtil.BodyEmailForForgetPassword(newPw);
                // send email
                await UserUtil.sendEmail("Welcome to PHONE SHOPPING", body, DTO.Email.Trim());
                user.Password = hashPw;
                user.UpdateAt = DateTime.Now;
                await daoUser.UpdateUser(user);
                return new ResponseDTO<bool>(true, "Password changed successful. Please check your email");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<UserDetailDTO?>> Update(Guid UserID, UserUpdateDTO DTO)
        {
            try
            {
                User? user = await daoUser.getUser(UserID);
                if (user == null)
                {
                    return new ResponseDTO<UserDetailDTO?>(null, "Not found user", (int)HttpStatusCode.NotFound);
                }
                user.FullName = DTO.FullName.Trim();
                user.Phone = DTO.Phone;
                user.Email = DTO.Email.Trim();
                UserDetailDTO data = mapper.Map<UserDetailDTO>(user);
                if (await daoUser.isExist(DTO.Email.Trim(), UserID))
                {
                    return new ResponseDTO<UserDetailDTO?>(data, "Email has existed", (int)HttpStatusCode.Conflict);
                }
                user.UpdateAt = DateTime.Now;
                await daoUser.UpdateUser(user);
                return new ResponseDTO<UserDetailDTO?>(data, "Update successful");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<bool>> ChangePassword(Guid UserID, ChangePasswordDTO DTO)
        {
            try
            {
                User? user = await daoUser.getUser(UserID);
                if (user == null)
                {
                    return new ResponseDTO<bool>(false, "Not found user", (int)HttpStatusCode.NotFound);
                }
                if (DTO.CurrentPassword == null)
                {
                    return new ResponseDTO<bool>(false, "Current password must not contain all space", (int)HttpStatusCode.Conflict);
                }
                if (DTO.ConfirmPassword == null)
                {
                    return new ResponseDTO<bool>(false, "Confirm password must not contain all space", (int)HttpStatusCode.Conflict);
                }
                if (DTO.NewPassword == null)
                {
                    return new ResponseDTO<bool>(false, "New password must not contain all space", (int)HttpStatusCode.Conflict);
                }
                if (user.Password != UserUtil.HashPassword(DTO.CurrentPassword))
                {
                    return new ResponseDTO<bool>(false, "Your old password not correct", (int)HttpStatusCode.Conflict);
                }
                if (!DTO.ConfirmPassword.Equals(DTO.NewPassword))
                {
                    return new ResponseDTO<bool>(false, "Your confirm password not the same new password", (int)HttpStatusCode.Conflict);
                }
                user.Password = UserUtil.HashPassword(DTO.NewPassword);
                user.UpdateAt = DateTime.Now;
                await daoUser.UpdateUser(user);
                return new ResponseDTO<bool>(true, "Change successful");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
