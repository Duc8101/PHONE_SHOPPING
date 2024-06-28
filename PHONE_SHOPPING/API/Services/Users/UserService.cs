using API.Services.Base;
using AutoMapper;
using Common.Base;
using Common.Const;
using Common.DTO.UserDTO;
using Common.Entity;
using Common.Enum;
using DataAccess.DBContext;
using DataAccess.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace API.Services.Users
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IMapper mapper, PhoneShoppingContext context) : base(mapper, context)
        {

        }
        public ResponseBase<UserDetailDTO?> Detail(Guid userId)
        {
            try
            {
                User? user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == userId);
                if (user == null)
                {
                    return new ResponseBase<UserDetailDTO?>(null, "Not found user", (int)HttpStatusCode.NotFound);
                }
                string AccessToken = getAccessToken(user);
                UserDetailDTO data = _mapper.Map<UserDetailDTO>(user);
                data.Token = AccessToken;
                return new ResponseBase<UserDetailDTO?>(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase<UserDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase<UserDetailDTO?> Login(LoginDTO DTO)
        {
            try
            {
                User? user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Username == DTO.Username && u.IsDeleted == false);
                if (user == null)
                {
                    return new ResponseBase<UserDetailDTO?>(null, "Username or password incorrect", (int)HttpStatusCode.NotFound);
                }
                string AccessToken = getAccessToken(user);
                UserDetailDTO data = _mapper.Map<UserDetailDTO>(user);
                data.Token = AccessToken;
                // ------------------------- remove all cart ------------------------- 
                List<Cart> list = _context.Carts.Where(c => c.UserId == user.UserId && c.IsCheckout == false && c.IsDeleted == false).ToList();
                foreach (Cart cart in list)
                {
                    cart.IsDeleted = true;
                    _context.Carts.Update(cart);
                    _context.SaveChanges();
                }
                return new ResponseBase<UserDetailDTO?>(data, string.Empty);

            }
            catch (Exception ex)
            {
                return new ResponseBase<UserDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        private string getAccessToken(User user)
        {
            byte[] key = Encoding.UTF8.GetBytes("Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx");
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //  create list claim  to store user's information
            List<Claim> list = new List<Claim>()
            {
                new Claim("id", user.UserId.ToString()),
            };
            JwtSecurityToken token = new JwtSecurityToken("JWTAuthenticationServer",
                "JWTServicePostmanClient", list, expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            // get access token
            return handler.WriteToken(token);
        }

        public ResponseBase<bool> Logout(Guid UserID)
        {
            try
            {
                List<Cart> list = _context.Carts.Where(c => c.UserId == UserID && c.IsCheckout == false && c.IsDeleted == false).ToList();
                foreach (Cart cart in list)
                {
                    cart.IsDeleted = true;
                    _context.Carts.Update(cart);
                    _context.SaveChanges();
                }
                return new ResponseBase<bool>(true, "Logout successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase<bool>> Create(UserCreateDTO DTO)
        {
            try
            {
                Regex regex = new Regex(UserConst.FORMAT_EMAIL);
                if (!regex.IsMatch(DTO.Email.Trim()))
                {
                    return new ResponseBase<bool>(false, "Invalid email", (int)HttpStatusCode.Conflict);
                }
                if (_context.Users.Any(u => u.Username == DTO.Username || u.Email == DTO.Email.Trim()))
                {
                    return new ResponseBase<bool>(false, "Username or email has existed", (int)HttpStatusCode.Conflict);
                }
                string newPw = UserUtil.RandomPassword();
                string hashPw = UserUtil.HashPassword(newPw);
                // get body email
                string body = UserUtil.BodyEmailForRegister(newPw);
                // send email
                await UserUtil.sendEmail("Welcome to PHONE SHOPPING", body, DTO.Email.Trim());
                User user = _mapper.Map<User>(DTO);
                user.UserId = Guid.NewGuid();
                user.Password = hashPw;
                user.RoleId = (int)RoleEnum.Customer;
                user.CreatedAt = DateTime.Now;
                user.UpdateAt = DateTime.Now;
                user.IsDeleted = false;
                _context.Users.Add(user);
                _context.SaveChanges();
                return new ResponseBase<bool>(true, "Register successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase<bool>> ForgotPassword(ForgotPasswordDTO DTO)
        {
            try
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == DTO.Email.Trim());
                if (user == null)
                {
                    return new ResponseBase<bool>(false, "Not found email", (int)HttpStatusCode.NotFound);
                }
                string newPw = UserUtil.RandomPassword();
                string hashPw = UserUtil.HashPassword(newPw);
                // get body email
                string body = UserUtil.BodyEmailForForgetPassword(newPw);
                // send email
                await UserUtil.sendEmail("Welcome to PHONE SHOPPING", body, DTO.Email.Trim());
                user.Password = hashPw;
                user.UpdateAt = DateTime.Now;
                _context.Users.Update(user);
                _context.SaveChanges();
                return new ResponseBase<bool>(true, "Password changed successful. Please check your email");
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase<UserDetailDTO?> Update(User user, UserUpdateDTO DTO)
        {
            try
            {
                user.FullName = DTO.FullName.Trim();
                user.Phone = DTO.Phone;
                user.Email = DTO.Email.Trim();
                UserDetailDTO data = _mapper.Map<UserDetailDTO>(user);
                if (_context.Users.Any(u => u.Email == DTO.Email.Trim() && u.UserId != user.UserId))
                {
                    return new ResponseBase<UserDetailDTO?>(data, "Email has existed", (int)HttpStatusCode.Conflict);
                }
                user.UpdateAt = DateTime.Now;
                _context.Users.Update(user);
                _context.SaveChanges();
                return new ResponseBase<UserDetailDTO?>(data, "Update successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase<UserDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase<bool> ChangePassword(User user, ChangePasswordDTO DTO)
        {
            try
            {
                if (DTO.CurrentPassword == null)
                {
                    return new ResponseBase<bool>(false, "Current password must not contain all space", (int)HttpStatusCode.Conflict);
                }
                if (DTO.ConfirmPassword == null)
                {
                    return new ResponseBase<bool>(false, "Confirm password must not contain all space", (int)HttpStatusCode.Conflict);
                }
                if (DTO.NewPassword == null)
                {
                    return new ResponseBase<bool>(false, "New password must not contain all space", (int)HttpStatusCode.Conflict);
                }
                if (user.Password != UserUtil.HashPassword(DTO.CurrentPassword))
                {
                    return new ResponseBase<bool>(false, "Your old password not correct", (int)HttpStatusCode.Conflict);
                }
                if (!DTO.ConfirmPassword.Equals(DTO.NewPassword))
                {
                    return new ResponseBase<bool>(false, "Your confirm password not the same new password", (int)HttpStatusCode.Conflict);
                }
                user.Password = UserUtil.HashPassword(DTO.NewPassword);
                user.UpdateAt = DateTime.Now;
                _context.Users.Update(user);
                _context.SaveChanges();
                return new ResponseBase<bool>(true, "Change successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
