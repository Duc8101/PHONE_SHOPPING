﻿using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using System.Net;

namespace MVC.Services
{
    public class LoginService : BaseService
    {

        public LoginService() : base()
        {

        }

        public async Task<ResponseDTO<UserListDTO?>> Index(string UserID)
        {
            try
            {
                string URL = "https://localhost:7033/User/Detail/" + UserID;
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO<UserListDTO?>? result = Deserialize<ResponseDTO<UserListDTO?>>(data);
                if (result == null)
                {
                    return new ResponseDTO<UserListDTO?>(null, data, (int)response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO<UserListDTO?>(result.Data, string.Empty);
                }
                return new ResponseDTO<UserListDTO?>(null, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserListDTO?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<UserListDTO?>> Index(LoginDTO DTO)
        {
            try
            {
                if (DTO.Password == null)
                {
                    return new ResponseDTO<UserListDTO?>(null, "Username or password incorrect", (int)HttpStatusCode.Conflict);
                }
                string URL = "https://localhost:7033/User/Login";
                string requestData = getRequestData<LoginDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
                string data = await getResponseData(response);
                ResponseDTO<UserListDTO?>? result = Deserialize<ResponseDTO<UserListDTO?>>(data);
                if (result == null)
                {
                    return new ResponseDTO<UserListDTO?>(null, data, (int)response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO<UserListDTO?>(result.Data, string.Empty);
                }
                return new ResponseDTO<UserListDTO?>(null, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserListDTO?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
