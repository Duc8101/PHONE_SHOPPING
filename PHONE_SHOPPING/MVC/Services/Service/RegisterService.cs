﻿using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using MVC.Services.IService;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MVC.Services.Service
{
    public class RegisterService : BaseService, IRegisterService
    {
        public RegisterService() : base()
        {
        }

        public async Task<ResponseDTO> Register(UserCreateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/User/Create";
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PostAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseDTO? result = Deserialize(responseData);
                if (result == null)
                {
                    return new ResponseDTO(false, responseData, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
