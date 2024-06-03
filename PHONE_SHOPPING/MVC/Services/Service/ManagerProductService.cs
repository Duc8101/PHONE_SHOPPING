using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class ManagerProductService : BaseService, IManagerProductService
    {
        public ManagerProductService() : base()
        {
        }

        private async Task<ResponseDTO> getListCategory()
        {
            try
            {
                string URL = "https://localhost:7033/Category/List/All";
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO? result = Deserialize<ResponseDTO>(data);
                if (result == null)
                {
                    return new ResponseDTO(null, data, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        private async Task<ResponseDTO> getPagedResult(string? name, int? CategoryID, int? page)
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
                ResponseDTO? result = Deserialize<ResponseDTO>(data);
                if (result == null)
                {
                    return new ResponseDTO(null, data, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO> Index(string? name, int? CategoryID, int? page)
        {
            try
            {
                ResponseDTO resCategory = await getListCategory();
                ResponseDTO resProduct = await getPagedResult(name, CategoryID, page);
                if (resCategory.Data == null)
                {
                    return new ResponseDTO(null, resCategory.Message, resCategory.Code);
                }
                if (resProduct.Data == null)
                {
                    return new ResponseDTO(null, resProduct.Message, resProduct.Code);
                }
                Dictionary<string, object> result = new Dictionary<string, object>();
                result["result"] = resProduct.Data;
                result["list"] = resCategory.Data;
                result["CategoryID"] = CategoryID == null ? 0 : CategoryID;
                result["name"] = name == null ? "" : name.Trim();
                return new ResponseDTO(result, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO> Create()
        {
            return await getListCategory();
        }
        public async Task<ResponseDTO> Create(ProductCreateUpdateDTO DTO)
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
                ResponseDTO? result = Deserialize<ResponseDTO>(responseData);
                ResponseDTO resCat = await getListCategory();
                if (resCat.Data == null)
                {
                    return new ResponseDTO(null, resCat.Message, resCat.Code);
                }
                if (result == null)
                {
                    return new ResponseDTO(null, responseData, (int)response.StatusCode);
                }
                if (result.Code == (int)HttpStatusCode.OK || result.Code == (int)HttpStatusCode.Conflict)
                {
                    return new ResponseDTO(resCat.Data, result.Message, (int)response.StatusCode);
                }
                return new ResponseDTO(null, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO> Update(Guid ProductID)
        {
            try
            {
                string URL = "https://localhost:7033/Product/Detail/" + ProductID;
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO? resultPro = Deserialize<ResponseDTO>(data);
                ResponseDTO resultCat = await getListCategory();
                if (resultCat.Data == null)
                {
                    return new ResponseDTO(null, resultCat.Message, resultCat.Code);
                }
                if (resultPro == null)
                {
                    return new ResponseDTO(null, data, (int)response.StatusCode);
                }
                if (resultPro.Data == null)
                {
                    return new ResponseDTO(null, resultPro.Message, (int)response.StatusCode);
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["product"] = resultPro.Data;
                dic["list"] = resultCat.Data;
                return new ResponseDTO(dic, resultPro.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO> Update(Guid ProductID, ProductCreateUpdateDTO DTO)
        {
            try
            {
                DTO.ProductName = DTO.ProductName == null ? "" : DTO.ProductName.Trim();
                DTO.Image = DTO.Image == null ? "" : DTO.Image.Trim();
                string URL = "https://localhost:7033/Product/Update/" + ProductID;
                string requestData = getRequestData<ProductCreateUpdateDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PutAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO? resultPro = Deserialize<ResponseDTO>(responseData);
                ResponseDTO resultCat = await getListCategory();
                if (resultCat.Data == null)
                {
                    return new ResponseDTO(null, resultCat.Message, resultCat.Code);
                }
                if (resultPro == null)
                {
                    return new ResponseDTO(null, responseData, (int)response.StatusCode);
                }
                if (resultPro.Data == null)
                {
                    return new ResponseDTO(null, resultPro.Message, (int)response.StatusCode);
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["product"] = resultPro.Data;
                dic["list"] = resultCat.Data;
                return new ResponseDTO(dic, resultPro.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO> Delete(Guid ProductID)
        {
            try
            {
                string URL = "https://localhost:7033/Product/Delete/" + ProductID;
                HttpResponseMessage response = await DeleteAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO resCategory = await getListCategory();
                ResponseDTO? resPro = Deserialize<ResponseDTO>(data);
                if (resCategory.Data == null)
                {
                    return new ResponseDTO(null, resCategory.Message, resCategory.Code);
                }
                if (resPro == null)
                {
                    return new ResponseDTO(null, data, (int)response.StatusCode);
                }
                if (resPro.Data == null)
                {
                    return new ResponseDTO(null, resPro.Message, (int)response.StatusCode);
                }
                Dictionary<string, object> result = new Dictionary<string, object>();
                result["result"] = resPro.Data;
                result["list"] = resCategory.Data;
                result["CategoryID"] = 0;
                result["name"] = "";
                return new ResponseDTO(result, resPro.Message);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
