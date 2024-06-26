﻿using Common.Base;
using Common.DTO.CartDTO;

namespace API.Services.Carts
{
    public interface ICartService
    {
        ResponseBase List(Guid UserID);
        ResponseBase Create(CartCreateDTO DTO, Guid userId);
        ResponseBase Delete(Guid productId, Guid userId);
    }
}
