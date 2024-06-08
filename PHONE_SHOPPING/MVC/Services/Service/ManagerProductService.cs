﻿using Common.Base;
using Common.DTO.CategoryDTO;
using Common.DTO.ProductDTO;
using Common.Pagination;
using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class ManagerProductService : BaseService, IManagerProductService
    {
        public ManagerProductService(HttpClient client) : base(client)
        {
        }

        private async Task<ResponseBase<List<CategoryListDTO>?>> getListCategory()
        {
            string URL = "https://localhost:7178/Category/List/All";
            return await Get<List<CategoryListDTO>?>(URL);
        }
        private async Task<ResponseBase<Pagination<ProductListDTO>?>> getPagedResult(string? name, int? CategoryID, int? page)
        {
            int pageSelected = page == null ? 1 : page.Value;
            string URL = "https://localhost:7178/Product/Manager/List";
            ResponseBase<Pagination<ProductListDTO>?> response;
            if (name == null)
            {
                if (CategoryID == null)
                {
                    response = await Get<Pagination<ProductListDTO>?>(URL, new KeyValuePair<string, object>("page", pageSelected));
                }
                else
                {
                    response = await Get<Pagination<ProductListDTO>?>(URL, new KeyValuePair<string, object>("CategoryID", CategoryID),
                        new KeyValuePair<string, object>("page", pageSelected));
                }
            }
            else if (CategoryID == null)
            {
                response = await Get<Pagination<ProductListDTO>?>(URL, new KeyValuePair<string, object>("name", name),
                    new KeyValuePair<string, object>("page", pageSelected));
            }
            else
            {
                response = await Get<Pagination<ProductListDTO>?>(URL, new KeyValuePair<string, object>("name", name),
                    new KeyValuePair<string, object>("CategoryID", CategoryID), new KeyValuePair<string, object>("page", pageSelected));
            }
            return response;
        }
        public async Task<ResponseBase<Dictionary<string, object>?>> Index(string? name, int? CategoryID, int? page)
        {
            ResponseBase<List<CategoryListDTO>?> resCategory = await getListCategory();
            ResponseBase<Pagination<ProductListDTO>?> resProduct = await getPagedResult(name, CategoryID, page);
            if (resCategory.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, resCategory.Message, resCategory.Code);
            }
            if (resProduct.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, resProduct.Message, resProduct.Code);
            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["result"] = resProduct.Data;
            result["list"] = resCategory.Data;
            result["CategoryID"] = CategoryID == null ? 0 : CategoryID;
            result["name"] = name == null ? "" : name.Trim();
            return new ResponseBase<Dictionary<string, object>?>(result, string.Empty);
        }
        public async Task<ResponseBase<List<CategoryListDTO>?>> Create()
        {
            return await getListCategory();
        }
        public async Task<ResponseBase<List<CategoryListDTO>?>> Create(ProductCreateUpdateDTO DTO)
        {
            DTO.ProductName = DTO.ProductName == null ? "" : DTO.ProductName.Trim();
            DTO.Image = DTO.Image == null ? "" : DTO.Image.Trim();
            string URL = "https://localhost:7178/Product/Create";
            ResponseBase<bool> response = await Post<ProductCreateUpdateDTO, bool>(URL, DTO);
            if (response.Data == false)
            {
                return new ResponseBase<List<CategoryListDTO>?>(null, response.Message, response.Code);
            }
            ResponseBase<List<CategoryListDTO>?> result = await getListCategory();
            if (result.Data == null)
            {
                return new ResponseBase<List<CategoryListDTO>?>(null, response.Message, response.Code);
            }
            if (response.Code == (int)HttpStatusCode.OK || response.Code == (int)HttpStatusCode.Conflict)
            {
                return new ResponseBase<List<CategoryListDTO>?>(result.Data, response.Message, response.Code);
            }
            return new ResponseBase<List<CategoryListDTO>?>(null, response.Message, response.Code);
        }
        public async Task<ResponseBase<Dictionary<string, object>?>> Update(Guid ProductID)
        {
            string URL = "https://localhost:7178/Product/Detail/" + ProductID;
            ResponseBase<ProductListDTO?> resPro = await Get<ProductListDTO?>(URL);
            ResponseBase<List<CategoryListDTO>?> resCat = await getListCategory();
            if (resCat.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, resCat.Message, resCat.Code);
            }
            if (resPro.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, resPro.Message, resPro.Code);
            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["product"] = resPro.Data;
            result["list"] = resCat.Data;
            return new ResponseBase<Dictionary<string, object>?>(result, string.Empty);
        }
        public async Task<ResponseBase<Dictionary<string, object>?>> Update(Guid ProductID, ProductCreateUpdateDTO DTO)
        {
            DTO.ProductName = DTO.ProductName == null ? "" : DTO.ProductName.Trim();
            DTO.Image = DTO.Image == null ? "" : DTO.Image.Trim();
            string URL = "https://localhost:7178/Product/Update/" + ProductID;
            ResponseBase<ProductListDTO?>? resPro = await Put<ProductCreateUpdateDTO, ProductListDTO?>(URL, DTO);
            ResponseBase<List<CategoryListDTO>?> resCat = await getListCategory();
            if (resCat.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, resCat.Message, resCat.Code);
            }

            if (resPro.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, resPro.Message, resPro.Code);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["product"] = resPro.Data;
            dic["list"] = resCat.Data;
            return new ResponseBase<Dictionary<string, object>?>(dic, resPro.Message, resPro.Code);

        }
        public async Task<ResponseBase<Dictionary<string, object>?>> Delete(Guid ProductID)
        {
            string URL = "https://localhost:7178/Product/Delete/" + ProductID;
            ResponseBase<bool> response = await Delete<bool>(URL);
            if (response.Data == false)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, response.Message, response.Code);
            }
            ResponseBase<Dictionary<string, object>?> result = await Index(null, null, null);
            if (result.Code == (int)HttpStatusCode.OK)
            {
                result.Message = response.Message;
            }
            return result;
        }
    }
}
