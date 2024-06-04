using DataAccess.Base;
using DataAccess.Const;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
using DataAccess.Pagination;
using MVC.Services.IService;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MVC.Services.Service
{
    public class ManagerProductService : BaseService, IManagerProductService
    {
        public ManagerProductService() : base()
        {
        }

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
                string URL = "https://localhost:7178/Product/Manager/List";
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
        public async Task<ResponseBase<List<CategoryListDTO>?>> Create()
        {
            return await getListCategory();
        }
        public async Task<ResponseBase<List<CategoryListDTO>?>> Create(ProductCreateUpdateDTO DTO)
        {
            try
            {
                DTO.ProductName = DTO.ProductName == null ? "" : DTO.ProductName.Trim();
                DTO.Image = DTO.Image == null ? "" : DTO.Image.Trim();
                string URL = "https://localhost:7178/Product/Create";
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PostAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseBase<bool>? result = Deserialize<bool>(responseData);
                if (result == null)
                {
                    return new ResponseBase<List<CategoryListDTO>?>(null, "Can't get data", (int)response.StatusCode);
                }
                ResponseBase<List<CategoryListDTO>?> resCat = await getListCategory();
                if (resCat.Data == null)
                {
                    return new ResponseBase<List<CategoryListDTO>?>(null, resCat.Message, resCat.Code);
                }
                if (result.Code == (int)HttpStatusCode.OK || result.Code == (int)HttpStatusCode.Conflict)
                {
                    return new ResponseBase<List<CategoryListDTO>?>(resCat.Data, result.Message, (int)response.StatusCode);
                }
                return new ResponseBase<List<CategoryListDTO>?>(null, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<CategoryListDTO>?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseBase<Dictionary<string, object>?>> Update(Guid ProductID)
        {
            try
            {
                string URL = "https://localhost:7178/Product/Detail/" + ProductID;
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<ProductListDTO?>? resultPro = Deserialize<ProductListDTO?>(data);
                ResponseBase<List<CategoryListDTO>?> resultCat = await getListCategory();
                if (resultCat.Data == null)
                {
                    return new ResponseBase<Dictionary<string, object>?>(null, resultCat.Message, resultCat.Code);
                }
                if (resultPro == null)
                {
                    return new ResponseBase<Dictionary<string, object>?>(null, "Can't get data", (int)response.StatusCode);
                }
                if (resultPro.Data == null)
                {
                    return new ResponseBase<Dictionary<string, object>?>(null, resultPro.Message, (int)response.StatusCode);
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["product"] = resultPro.Data;
                dic["list"] = resultCat.Data;
                return new ResponseBase<Dictionary<string, object>?>(dic, resultPro.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseBase<Dictionary<string, object>?>> Update(Guid ProductID, ProductCreateUpdateDTO DTO)
        {
            try
            {
                DTO.ProductName = DTO.ProductName == null ? "" : DTO.ProductName.Trim();
                DTO.Image = DTO.Image == null ? "" : DTO.Image.Trim();
                string URL = "https://localhost:7178/Product/Update/" + ProductID;
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PutAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseBase<ProductListDTO?>? resultPro = Deserialize<ProductListDTO?>(responseData);
                ResponseBase<List<CategoryListDTO>?> resultCat = await getListCategory();
                if (resultPro == null)
                {
                    return new ResponseBase<Dictionary<string, object>?>(null, "Can't get data", (int)response.StatusCode);
                }
                if (resultCat.Data == null)
                {
                    return new ResponseBase<Dictionary<string, object>?>(null, resultCat.Message, resultCat.Code);
                }

                if (resultPro.Data == null)
                {
                    return new ResponseBase<Dictionary<string, object>?>(null, resultPro.Message, (int)response.StatusCode);
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["product"] = resultPro.Data;
                dic["list"] = resultCat.Data;
                return new ResponseBase<Dictionary<string, object>?>(dic, resultPro.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseBase<Dictionary<string, object>?>> Delete(Guid ProductID)
        {
            try
            {
                string URL = "https://localhost:7178/Product/Delete/" + ProductID;
                HttpResponseMessage response = await client.DeleteAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<bool>? res = Deserialize<bool>(data);
                if (res == null)
                {
                    return new ResponseBase<Dictionary<string, object>?>(null, data, (int)response.StatusCode);
                }
                if (res.Data == false)
                {
                    return new ResponseBase<Dictionary<string, object>?>(null, res.Message, res.Code);
                }
                ResponseBase<Dictionary<string, object>?> result = await Index(null, null, null);
                if(result.Code == (int)HttpStatusCode.OK)
                {
                    result.Message = res.Message;
                } 
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
