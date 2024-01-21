using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
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
        public async Task<ResponseDTO<List<CategoryListDTO>?>> Create(ProductCreateUpdateDTO DTO)
        {
            try
            {
                DTO.ProductName = DTO.ProductName == null ? "" : DTO.ProductName.Trim();
                DTO.Image = DTO.Image == null ? "" : DTO.Image.Trim();
                string URL = "https://localhost:7033/Product/Create";
                string requestData = getRequestData<ProductCreateUpdateDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO<bool>? result = Deserialize<ResponseDTO<bool>>(responseData);
                ResponseDTO<List<CategoryListDTO>?> resCat = await getListCategory();
                if(resCat.Data == null)
                {
                    return new ResponseDTO<List<CategoryListDTO>?>(null, resCat.Message, resCat.Code);
                }
                if (result == null)
                {
                    return new ResponseDTO<List<CategoryListDTO>?>(null, responseData, (int)response.StatusCode);
                }
                if(result.Code == (int) HttpStatusCode.OK || result.Code == (int)HttpStatusCode.Conflict)
                {
                    return new ResponseDTO<List<CategoryListDTO>?>(resCat.Data, result.Message, (int)response.StatusCode);
                }
                return new ResponseDTO<List<CategoryListDTO>?>(null, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<CategoryListDTO>?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO<Dictionary<string, object>?>> Update(Guid ProductID)
        {
            try
            {
                string URL = "https://localhost:7033/Product/Detail/" + ProductID;
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO<ProductListDTO?>? resultPro = Deserialize<ResponseDTO<ProductListDTO?>>(data);
                ResponseDTO<List<CategoryListDTO>?> resultCat = await getListCategory();
                if(resultCat.Data == null)
                {
                    return new ResponseDTO<Dictionary<string, object>?>(null, resultCat.Message, resultCat.Code);
                }
                if (resultPro == null)
                {
                    return new ResponseDTO<Dictionary<string, object>?>(null , data, (int)response.StatusCode);
                }
                if(resultPro.Data == null)
                {
                    return new ResponseDTO<Dictionary<string, object>?>(null, resultPro.Message, (int)response.StatusCode);
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["product"] = resultPro.Data;
                dic["list"] = resultCat.Data;
                return new ResponseDTO<Dictionary<string, object>?>(dic, resultPro.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<Dictionary<string, object>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
