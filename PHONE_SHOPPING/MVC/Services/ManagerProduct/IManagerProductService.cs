﻿using Common.Base;
using Common.DTO.CategoryDTO;
using Common.DTO.ProductDTO;

namespace MVC.Services.ManagerProduct
{
    public interface IManagerProductService
    {
        Task<ResponseBase<Dictionary<string, object>?>> Index(string? name, int? categoryId, int? page);
        Task<ResponseBase<List<CategoryListDTO>?>> Create();
        Task<ResponseBase<List<CategoryListDTO>?>> Create(ProductCreateUpdateDTO DTO);
        Task<ResponseBase<Dictionary<string, object>?>> Update(Guid productId);
        Task<ResponseBase<Dictionary<string, object>?>> Update(Guid productId, ProductCreateUpdateDTO DTO);
        Task<ResponseBase<Dictionary<string, object>?>> Delete(Guid productId);
    }
}
