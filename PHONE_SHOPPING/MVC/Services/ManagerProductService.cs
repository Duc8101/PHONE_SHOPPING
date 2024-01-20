using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
using DataAccess.DTO;
using System.Net;

namespace MVC.Services
{
    public class ManagerProductService : BaseService
    {
        private async Task<ResponseDTO<List<CategoryListDTO>?>> getListCategory()
        {
            try
            {
                string URL = "https://localhost:7033/Category/List/All";
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO<List<CategoryListDTO>?>? result = Deserialize<ResponseDTO<List<CategoryListDTO>?>>(data);
                if (result == null)
                {
                    return new ResponseDTO<List<CategoryListDTO>?>(null, data, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<CategoryListDTO>?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        private async Task<ResponseDTO<PagedResultDTO<ProductListDTO>?>> getPagedResult(string? name, int? CategoryID, int? page)
        {
            try
            {
                int pageSelected = page == null ? 1 : page.Value;
                string URL = "https://localhost:7033/Product/List";
                if (CategoryID == null && name == null)
                {
                    URL = URL + "?isAdmin=true&page=" + pageSelected;
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
                    URL = URL + "&isAdmin=true&page=" + pageSelected;
                }
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO<PagedResultDTO<ProductListDTO>?>? result = Deserialize<ResponseDTO<PagedResultDTO<ProductListDTO>?>>(data);
                if (result == null)
                {
                    return new ResponseDTO<PagedResultDTO<ProductListDTO>?>(null, data, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO<PagedResultDTO<ProductListDTO>?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO<Dictionary<string, object>?>> Index(string? name, int? CategoryID, int? page)
        {
            try
            {
                ResponseDTO<List<CategoryListDTO>?> resCategory = await getListCategory();
                ResponseDTO<PagedResultDTO<ProductListDTO>?> resProduct = await getPagedResult(name, CategoryID, page);
                if (resCategory.Data == null)
                {
                    return new ResponseDTO<Dictionary<string, object>?>(null, resCategory.Message, resCategory.Code);
                }
                if (resProduct.Data == null)
                {
                    return new ResponseDTO<Dictionary<string, object>?>(null, resProduct.Message, resProduct.Code);
                }
                Dictionary<string, object> result = new Dictionary<string, object>();
                result["result"] = resProduct.Data;
                result["list"] = resCategory.Data;
                result["CategoryID"] = CategoryID == null ? 0 : CategoryID;
                result["name"] = name == null ? "" : name.Trim();
                return new ResponseDTO<Dictionary<string, object>?>(result, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<Dictionary<string, object>?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO<List<CategoryListDTO>?>> Create()
        {
            return await getListCategory();
        }

    }
}
