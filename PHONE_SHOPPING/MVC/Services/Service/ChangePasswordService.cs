﻿using DataAccess.Base;
using DataAccess.DTO.UserDTO;
using MVC.Services.IService;

namespace MVC.Services.Service
{
    public class ChangePasswordService : BaseService, IChangePasswordService
    {
        public ChangePasswordService(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseBase<bool>> Index(ChangePasswordDTO DTO)
        {
            string URL = "https://localhost:7178/User/ChangePassword";
            return await Put<ChangePasswordDTO, bool>(URL, DTO);
        }
    }
}
