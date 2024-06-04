using DataAccess.Base;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
using DataAccess.Pagination;
using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class HomeService : BaseService, IHomeService
    {
        private async Task<ResponseBase<List<CategoryListDTO>?>> getListCategory()
        {
            try
            {
                string URL = "https://localhost:7178/Category/List/All";
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<List<CategoryListDTO>?>? result = Deserialize<List<CategoryListDTO>?>(data);
                if (result == null)
                {
                    return new ResponseBase<List<CategoryListDTO>?>(null, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<CategoryListDTO>?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        private async Task<ResponseBase<Pagination<ProductListDTO>?>> getPagedResult(string? name, int? CategoryID, int? page)
        {
            try
            {
                int pageSelected = page == null ? 1 : page.Value;
                string URL = "https://localhost:7178/Product/Home/List";
                if (CategoryID == null && name == null)
                {
                    URL = URL + "?page=" + pageSelected;
                }
                else
                {
                    if (name == null)
                    {
                        URL = URL + "?CategoryID=" + CategoryID;
                    }
                    else if (CategoryID == null)
                    {
                        URL = URL + "?name=" + name;
                    }
                    else
                    {
                        URL = URL + "?name=" + name + "&CategoryID=" + CategoryID;
                    }
                    URL = URL + "&page=" + pageSelected;
                }
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<Pagination<ProductListDTO>?>? result = Deserialize<Pagination<ProductListDTO>?>(data);
                if (result == null)
                {
                    return new ResponseBase<Pagination<ProductListDTO>?>(null, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<Pagination<ProductListDTO>?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase<Dictionary<string, object>?>> Index(string? name, int? CategoryID, int? page)
        {
            try
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
            catch (Exception ex)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
