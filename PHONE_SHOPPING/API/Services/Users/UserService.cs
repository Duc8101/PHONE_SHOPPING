﻿using API.Services.Base;
using AutoMapper;
using Common.Base;
using Common.DTO.UserDTO;
using Common.Entity;
using Common.Enum;
using DataAccess.DBContext;
using DataAccess.Helper;
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
        public UserService(IMapper mapper, PHONE_STOREContext context) : base(mapper, context)
        {

        }
        public ResponseBase Detail(Guid userId)
        {
            try
            {
                User? user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == userId);
                if (user == null)
                {
                    return new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
                }
                UserDetailDTO data = _mapper.Map<UserDetailDTO>(user);
                return new ResponseBase(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Login(LoginDTO DTO)
        {
            try
            {
                User? user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Username == DTO.Username);
                if (user == null || !user.Password.Equals(UserHelper.HashPassword(DTO.Password)))
                {
                    return new ResponseBase("Username or password incorrect", (int)HttpStatusCode.NotFound);
                }
                int clientId;
                Client? client = _context.Clients.FirstOrDefault(c => c.HarewareInfo == DTO.HarewareInfo);
                // nếu chưa đăng ký thiết bị
                if (client == null)
                {
                    client = new Client()
                    {
                        HarewareInfo = DTO.HarewareInfo,
                        CreatedAt = DateTime.Now,
                        UpdateAt = DateTime.Now,
                        IsDeleted = false,
                    };
                    _context.Clients.Add(client);
                    _context.SaveChanges();
                    clientId = client.ClientId;
                }
                else
                {
                    clientId = client.ClientId;
                }
                string AccessToken = getAccessToken(user);
                UserClient? userClient = _context.UserClients.FirstOrDefault(uc => uc.UserId == user.UserId && uc.ClientId == clientId);
                // nếu chưa đăng nhập trên thiết bị
                if (userClient == null)
                {
                    userClient = new UserClient()
                    {
                        UserClientId = Guid.NewGuid(),
                        UserId = user.UserId,
                        ClientId = clientId,
                        Token = AccessToken,
                        ExpireDate = DateTime.Now.AddDays(1),
                        CreatedAt = DateTime.Now,
                        UpdateAt = DateTime.Now,
                        IsDeleted = false,
                    };
                    _context.UserClients.Add(userClient);
                    _context.SaveChanges();
                }
                else
                {
                    userClient.Token = AccessToken;
                    userClient.UpdateAt = DateTime.Now;
                    userClient.ExpireDate = DateTime.Now.AddDays(1);
                    _context.UserClients.Update(userClient);
                    _context.SaveChanges();
                }
                UserLoginInfoDTO data = new UserLoginInfoDTO()
                {
                    Access_Token = AccessToken,
                    UserId = user.UserId,
                    RoleId = user.RoleId,
                    Username = user.Username,
                    ExpireDate = userClient.ExpireDate,
                };
                // ------------------------- remove all cart ------------------------- 
                List<Cart> list = _context.Carts.Where(c => c.UserId == user.UserId && c.IsCheckOut == false && c.IsDeleted == false).ToList();
                foreach (Cart cart in list)
                {
                    cart.IsDeleted = true;
                    _context.Carts.Update(cart);
                    _context.SaveChanges();
                }
                return new ResponseBase(data, string.Empty);

            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
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

        public ResponseBase Logout(Guid UserID)
        {
            try
            {
                List<Cart> list = _context.Carts.Where(c => c.UserId == UserID && c.IsCheckOut == false && c.IsDeleted == false).ToList();
                foreach (Cart cart in list)
                {
                    cart.IsDeleted = true;
                    _context.Carts.Update(cart);
                    _context.SaveChanges();
                }
                return new ResponseBase(true, "Logout successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase> Create(UserCreateDTO DTO)
        {
            try
            {
                Regex regex = new Regex("^[a-zA-Z][\\w-]+@([\\w]+\\.[\\w]+|[\\w]+\\.[\\w]{2,}\\.[\\w]{2,})");
                if (!regex.IsMatch(DTO.Email.Trim()))
                {
                    return new ResponseBase(false, "Invalid email", (int)HttpStatusCode.Conflict);
                }
                if (_context.Users.Any(u => u.Username == DTO.Username || u.Email == DTO.Email.Trim()))
                {
                    return new ResponseBase(false, "Username or email has existed", (int)HttpStatusCode.Conflict);
                }
                string newPw = UserHelper.RandomPassword();
                string hashPw = UserHelper.HashPassword(newPw);
                // get body email
                string body = UserHelper.BodyEmailForRegister(newPw);
                // send email
                await UserHelper.sendEmail("Welcome to PHONE SHOPPING", body, DTO.Email.Trim());
                User user = _mapper.Map<User>(DTO);
                user.UserId = Guid.NewGuid();
                user.Password = hashPw;
                user.RoleId = (int)RoleEnum.Customer;
                user.CreatedAt = DateTime.Now;
                user.UpdateAt = DateTime.Now;
                user.IsDeleted = false;
                _context.Users.Add(user);
                _context.SaveChanges();
                return new ResponseBase(true, "Register successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase> ForgotPassword(ForgotPasswordDTO DTO)
        {
            try
            {
                User? user = _context.Users.FirstOrDefault(u => u.Email == DTO.Email.Trim());
                if (user == null)
                {
                    return new ResponseBase(false, "Not found email", (int)HttpStatusCode.NotFound);
                }
                string newPw = UserHelper.RandomPassword();
                string hashPw = UserHelper.HashPassword(newPw);
                // get body email
                string body = UserHelper.BodyEmailForForgetPassword(newPw);
                // send email
                await UserHelper.sendEmail("Welcome to PHONE SHOPPING", body, DTO.Email.Trim());
                user.Password = hashPw;
                user.UpdateAt = DateTime.Now;
                _context.Users.Update(user);
                _context.SaveChanges();
                return new ResponseBase(true, "Password changed successful. Please check your email");
            }
            catch (Exception ex)
            {
                return new ResponseBase(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Update(User user, UserUpdateDTO DTO)
        {
            try
            {
                user.FullName = DTO.FullName.Trim();
                user.Phone = DTO.Phone;
                user.Email = DTO.Email.Trim();
                UserDetailDTO data = _mapper.Map<UserDetailDTO>(user);
                if (_context.Users.Any(u => u.Email == DTO.Email.Trim() && u.UserId != user.UserId))
                {
                    return new ResponseBase(data, "Email has existed", (int)HttpStatusCode.Conflict);
                }
                user.UpdateAt = DateTime.Now;
                _context.Users.Update(user);
                _context.SaveChanges();
                return new ResponseBase(data, "Update successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase ChangePassword(User user, ChangePasswordDTO DTO)
        {
            try
            {
                if (DTO.CurrentPassword == null)
                {
                    return new ResponseBase(false, "Current password must not contain all space", (int)HttpStatusCode.Conflict);
                }
                if (DTO.ConfirmPassword == null)
                {
                    return new ResponseBase(false, "Confirm password must not contain all space", (int)HttpStatusCode.Conflict);
                }
                if (DTO.NewPassword == null)
                {
                    return new ResponseBase(false, "New password must not contain all space", (int)HttpStatusCode.Conflict);
                }
                if (user.Password != UserHelper.HashPassword(DTO.CurrentPassword))
                {
                    return new ResponseBase(false, "Your old password not correct", (int)HttpStatusCode.Conflict);
                }
                if (!DTO.ConfirmPassword.Equals(DTO.NewPassword))
                {
                    return new ResponseBase(false, "Your confirm password not the same new password", (int)HttpStatusCode.Conflict);
                }
                user.Password = UserHelper.HashPassword(DTO.NewPassword);
                user.UpdateAt = DateTime.Now;
                _context.Users.Update(user);
                _context.SaveChanges();
                return new ResponseBase(true, "Change successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase GetUserByToken(string token, string hardware)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadJwtToken(token);
                string userId = tokenS.Claims.First(c => c.Type == "id").Value;
                User? user = _context.Users.Find(Guid.Parse(userId));
                if (user == null)
                {
                    return new ResponseBase("Not found user", (int) HttpStatusCode.NotFound);
                }
                Client? client = _context.Clients.FirstOrDefault(c => c.HarewareInfo == hardware);
                if(client == null)
                {
                    return new ResponseBase("Not found client", (int)HttpStatusCode.NotFound);
                }
                UserClient? userClient = _context.UserClients.FirstOrDefault(uc => uc.UserId == Guid.Parse(userId) 
                && uc.ClientId == client.ClientId);
                if(userClient == null)
                {
                    return new ResponseBase("User not register on this client", (int)HttpStatusCode.NotFound);
                }
                if(userClient.Token != token)
                {
                    return new ResponseBase("Invalid token", (int)HttpStatusCode.Conflict);
                }
                if(userClient.ExpireDate < DateTime.Now)
                {
                    return new ResponseBase("Token expired", (int)HttpStatusCode.Conflict);
                }
                UserLoginInfoDTO data = new UserLoginInfoDTO()
                {
                    Access_Token = token,
                    UserId = user.UserId,
                    RoleId = user.RoleId,
                    Username = user.Username,
                    ExpireDate = userClient.ExpireDate,
                };
                return new ResponseBase(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}