using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using MVC.Services.IService;
using System.Net;

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
        public async Task<ResponseDTO> Create(CategoryCreateUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Category/Create";
                string requestData = getRequestData<CategoryCreateUpdateDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO? result = Deserialize<ResponseDTO>(responseData);
                if (result == null)
                {
                    return new ResponseDTO(false, responseData, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO> Update(int ID)
        {
            try
            {
                string URL = "https://localhost:7033/Category/Detail/" + ID;
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
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO> Update(int ID, CategoryCreateUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Category/Update/" + ID;
                string requestData = getRequestData<CategoryCreateUpdateDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PutAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO? result = Deserialize<ResponseDTO>(responseData);
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
