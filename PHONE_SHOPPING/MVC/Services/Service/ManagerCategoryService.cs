using DataAccess.Base;
using DataAccess.Const;
using DataAccess.DTO.CategoryDTO;
using DataAccess.Pagination;
using MVC.Services.IService;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MVC.Services.Service
{
    public class ManagerCategoryService : BaseService, IManagerCategoryService
    {
        public ManagerCategoryService() : base()
        {
        }

        public async Task<ResponseBase<Pagination<CategoryListDTO>?>> Index(string? name, int? page)
        {
            try
            {
                int pageSelected = page == null ? 1 : page.Value;
                string URL = "https://localhost:7178/Category/List/Paged";
                if (name == null || name.Trim().Length == 0)
                {
                    URL = URL + "?page=" + pageSelected;
                }
                else
                {
                    URL = URL + "?name=" + name.Trim() + "&page=" + pageSelected;
                }
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<Pagination<CategoryListDTO>?>? result = Deserialize<Pagination<CategoryListDTO>?>(data);
                if (result == null)
                {
                    return new ResponseBase<Pagination<CategoryListDTO>?>(null, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<Pagination<CategoryListDTO>?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseBase<bool>> Create(CategoryCreateUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7178/Category/Create";
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PostAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseBase<bool>? result = Deserialize<bool>(responseData);
                if (result == null)
                {
                    return new ResponseBase<bool>(false, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseBase<CategoryListDTO?>> Update(int ID)
        {
            try
            {
                string URL = "https://localhost:7178/Category/Detail/" + ID;
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<CategoryListDTO?>? result = Deserialize<CategoryListDTO?>(data);
                if (result == null)
                {
                    return new ResponseBase<CategoryListDTO?>(null, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<CategoryListDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseBase<CategoryListDTO?>> Update(int ID, CategoryCreateUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7178/Category/Update/" + ID;
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PutAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseBase<CategoryListDTO?>? result = Deserialize<CategoryListDTO?>(responseData);
                if (result == null)
                {
                    return new ResponseBase<CategoryListDTO?>(null, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<CategoryListDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
