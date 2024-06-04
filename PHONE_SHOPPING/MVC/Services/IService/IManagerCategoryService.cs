﻿using DataAccess.Base;
using DataAccess.DTO.CategoryDTO;
using DataAccess.Pagination;

namespace MVC.Services.IService
{
    public interface IManagerCategoryService
    {
        Task<ResponseBase<Pagination<CategoryListDTO>?>> Index(string? name, int? page);
        Task<ResponseBase<bool>> Create(CategoryCreateUpdateDTO DTO);
        Task<ResponseBase<CategoryListDTO?>> Update(int ID);
        Task<ResponseBase<CategoryListDTO?>> Update(int ID, CategoryCreateUpdateDTO DTO);
    }
}
