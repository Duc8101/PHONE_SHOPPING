using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
using System.Net;

namespace MVC.Services
{
    public class HomeService : BaseService
    {
        private async Task<List<CategoryListDTO>?> getListCategory()
        {
            string URL = "https://localhost:7033/Category/List/All";
            HttpResponseMessage response = await GetAsync(URL);
            string data = await getResponseData(response);
            if(response.IsSuccessStatusCode)
            {
                ResponseDTO<List<CategoryListDTO>?>? result = Deserialize<ResponseDTO<List<CategoryListDTO>?>>(data);
                return result?.Data;
            }
            return null;
        }
        private async Task<PagedResultDTO<ProductListDTO>?> getPagedResult(string? name, int? CategoryID, int? page)
        {
            int pageSelected = page == null ? 1 : page.Value;
            string URL = "https://localhost:7033/Product/List";
            if(CategoryID == null && name == null)
            {
                URL = URL + "?page=" + pageSelected;
            }
            else
            {
                if(name == null)
                {
                    URL = URL + "?CategoryID=" + CategoryID;
                }else if(CategoryID == null)
                {
                    URL = URL + "?name=" + name;
                }
                else
                {
                    URL = URL + "?name=" + name + "&CategoryID=" + CategoryID;
                }
                URL = URL +  "&page=" + pageSelected;
            }
            HttpResponseMessage response = await GetAsync(URL);
            string data = await getResponseData(response);
            if(response.IsSuccessStatusCode)
            {
                ResponseDTO<PagedResultDTO<ProductListDTO>?>? result = Deserialize<ResponseDTO<PagedResultDTO<ProductListDTO>?>>(data);
                return result?.Data;
            }
            return null;
        }
        public async Task<ResponseDTO<Dictionary<string, object>?>> Index(string? name, int? CategoryID, int? page)
        {
            try
            {
                List<CategoryListDTO>? list = await getListCategory();
                PagedResultDTO<ProductListDTO>? paged = await getPagedResult(name, CategoryID, page);
                if(list == null)
                {
                    return new ResponseDTO<Dictionary<string, object>?>(null, "Get list category failed", (int)HttpStatusCode.InternalServerError);
                }
                if (paged == null)
                {
                    return new ResponseDTO<Dictionary<string, object>?>(null, "Get paged product failed", (int)HttpStatusCode.InternalServerError);
                }
                Dictionary<string, object> result = new Dictionary<string, object>();
                result["result"] = paged;
                result["list"] = list;
                result["CategoryID"] = CategoryID == null ? 0 : CategoryID;
                result["name"] = name == null ? "" : name.Trim();
                return new ResponseDTO<Dictionary<string, object>?>(result, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<Dictionary<string, object>?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
