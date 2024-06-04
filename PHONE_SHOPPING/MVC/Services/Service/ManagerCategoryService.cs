using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
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

        public async Task<ResponseDTO> Index(string? name, int? page)
        {
            try
            {
                int pageSelected = page == null ? 1 : page.Value;
                string URL = "https://localhost:7033/Category/List/Paged";
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
                ResponseDTO? result = Deserialize(data);
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
        public async Task<ResponseDTO> Create(CategoryCreateUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Category/Create";
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PostAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseDTO? result = Deserialize(responseData);
                if (result == null)
                {
                    return new ResponseDTO(null, responseData, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO> Update(int ID)
        {
            try
            {
                string URL = "https://localhost:7033/Category/Detail/" + ID;
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseDTO? result = Deserialize(data);
                if (result == null)
                {
                    return new ResponseDTO(null, data, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO> Update(int ID, CategoryCreateUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Category/Update/" + ID;
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PutAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseDTO? result = Deserialize(responseData);
                if (result == null)
                {
                    return new ResponseDTO(null, responseData, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
