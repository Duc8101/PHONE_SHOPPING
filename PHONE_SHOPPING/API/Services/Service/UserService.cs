using API.Services.IService;
using AutoMapper;
using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using DataAccess.Entity;
using DataAccess.Enum;
using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace API.Services.Service
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IMapper mapper, PHONE_SHOPPINGContext context) : base(mapper, context)
        {

        }
        public async Task<ResponseDTO<UserDetailDTO?>> Detail(Guid UserID)
        {
            try
            {
                User? user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == UserID);
                if (user == null)
                {
                    return new ResponseDTO<UserDetailDTO?>(null, "Not found user", (int)HttpStatusCode.NotFound);
                }
                string AccessToken = getAccessToken(user);
                UserDetailDTO data = _mapper.Map<UserDetailDTO>(user);
                data.Token = AccessToken;
                UserDetailDTO DTO = _mapper.Map<UserDetailDTO>(user);
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
                User? user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == DTO.Username && u.IsDeleted == false);
                if (user == null)
                {
                    return new ResponseDTO<UserDetailDTO?>(null, "Username or password incorrect", (int)HttpStatusCode.NotFound);
                }
                string AccessToken = getAccessToken(user);
                UserDetailDTO data = _mapper.Map<UserDetailDTO>(user);
                data.Token = AccessToken;
                // ------------------------- remove all cart ------------------------- 
                List<Cart> list = await _context.Carts.Where(c => c.UserId == user.UserId && c.IsCheckout == false && c.IsDeleted == false).ToListAsync();
                foreach (Cart cart in list)
                {
                    cart.IsDeleted = true;
                    _context.Carts.Update(cart);
                    await _context.SaveChangesAsync();
                }
                return new ResponseDTO<UserDetailDTO?>(data, string.Empty);

            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
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

        public async Task<ResponseDTO<bool>> Logout(Guid UserID)
        {
            try
            {
                List<Cart> list = await _context.Carts.Where(c => c.UserId == UserID && c.IsCheckout == false && c.IsDeleted == false).ToListAsync();
                foreach (Cart cart in list)
                {
                    cart.IsDeleted = true;
                    _context.Carts.Update(cart);
                    await _context.SaveChangesAsync();
                }
                return new ResponseDTO<bool>(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<bool>> Create(UserCreateDTO DTO)
        {
            try
            {
                Regex regex = new Regex(UserConst.FORMAT_EMAIL);
                if (!regex.IsMatch(DTO.Email.Trim()))
                {
                    return new ResponseDTO<bool>(false, "Invalid email", (int)HttpStatusCode.Conflict);
                }
                if (await _context.Users.AnyAsync(u => u.Username == DTO.Username || u.Email == DTO.Email.Trim()))
                {
                    return new ResponseDTO<bool>(false, "Username or email has existed", (int)HttpStatusCode.Conflict);
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
                user.RoleId = (int) RoleEnum.Customer;
                user.CreatedAt = DateTime.Now;
                user.UpdateAt = DateTime.Now;
                user.IsDeleted = false;
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
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
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == DTO.Email.Trim());
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
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
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
                User? user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == UserID);
                if (user == null)
                {
                    return new ResponseDTO<UserDetailDTO?>(null, "Not found user", (int)HttpStatusCode.NotFound);
                }
                user.FullName = DTO.FullName.Trim();
                user.Phone = DTO.Phone;
                user.Email = DTO.Email.Trim();
                UserDetailDTO data = _mapper.Map<UserDetailDTO>(user);
                if (await _context.Users.AnyAsync(u => u.Email == DTO.Email.Trim() && u.UserId != UserID))
                {
                    return new ResponseDTO<UserDetailDTO?>(data, "Email has existed", (int)HttpStatusCode.Conflict);
                }
                user.UpdateAt = DateTime.Now;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
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
                User? user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == UserID);
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
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return new ResponseDTO<bool>(true, "Change successful");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
