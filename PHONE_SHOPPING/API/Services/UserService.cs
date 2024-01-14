﻿using AutoMapper;
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
        public async Task<ResponseDTO<UserListDTO?>> Detail(Guid UserID)
        {
            try
            {
                User? user = await daoUser.getUser(UserID);
                if (user == null)
                {
                    return new ResponseDTO<UserListDTO?>(null, "Not found user", (int)HttpStatusCode.NotFound);
                }
                UserListDTO DTO = mapper.Map<UserListDTO>(user);
                return new ResponseDTO<UserListDTO?>(DTO, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserListDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<UserListDTO?>> Login(LoginDTO DTO)
        {
            try
            {
                User? user = await daoUser.getUser(DTO);
                if (user == null)
                {
                    return new ResponseDTO<UserListDTO?>(null, "Username or password incorrect", (int)HttpStatusCode.Conflict);
                }
                UserListDTO data = mapper.Map<UserListDTO>(user);
                return new ResponseDTO<UserListDTO?>(data, string.Empty);

            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserListDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<bool>> Logout(Guid UserID)
        {
            try
            {
                User? user = await daoUser.getUser(UserID);
                if (user == null)
                {
                    return new ResponseDTO<bool>(false, "Not found user", (int)HttpStatusCode.NotFound);
                }
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
    }
}
