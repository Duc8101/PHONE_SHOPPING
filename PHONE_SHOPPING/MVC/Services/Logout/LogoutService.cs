﻿using Common.Base;
using MVC.Services.Base;

namespace MVC.Services.Logout
{
    public class LogoutService : BaseService, ILogoutService
    {
        public async Task<ResponseBase<bool?>> Index()
        {
            string URL = "https://localhost:7077/User/Logout";
            return await Get<bool?>(URL);
        }
    }
}