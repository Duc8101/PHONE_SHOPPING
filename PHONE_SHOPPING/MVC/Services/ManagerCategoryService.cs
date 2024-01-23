using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using System.Net;

namespace MVC.Services
{
    public class ManagerCategoryService : BaseService
    {
        public async Task<ResponseDTO<PagedResultDTO<CategoryListDTO>?>> Index(string? name, int? page)
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
                ResponseDTO<PagedResultDTO<CategoryListDTO>?>? result = Deserialize<ResponseDTO<PagedResultDTO<CategoryListDTO>?>>(data);
                if (result == null)
                {
                    return new ResponseDTO<PagedResultDTO<CategoryListDTO>?>(null, data, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO<PagedResultDTO<CategoryListDTO>?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO<bool>> Create(CategoryCreateUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Category/Create";
                string requestData = getRequestData<CategoryCreateUpdateDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO<bool>? result = Deserialize<ResponseDTO<bool>>(responseData);
                if (result == null)
                {
                    return new ResponseDTO<bool>(false, responseData, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
