using DataAccess.Base;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
using DataAccess.Pagination;
using MVC.Services.IService;

namespace MVC.Services.Service
{
    public class HomeService : BaseService, IHomeService
    {
        public HomeService(HttpClient client) : base(client)
        {

        }
        private async Task<ResponseBase<List<CategoryListDTO>?>> getListCategory()
        {
            string URL = "https://localhost:7178/Category/List/All";
            return await Get<List<CategoryListDTO>?>(URL);
        }

        private async Task<ResponseBase<Pagination<ProductListDTO>?>> getPagedResult(string? name, int? CategoryID, int? page)
        {
            int pageSelected = page == null ? 1 : page.Value;
            string URL = "https://localhost:7178/Product/Home/List";
            ResponseBase<Pagination<ProductListDTO>?> response;
            if (name == null)
            {
                if (CategoryID == null)
                {
                    response = await Get<Pagination<ProductListDTO>?>(URL, new KeyValuePair<string, object>("page", pageSelected));
                }
                else
                {
                    response = await Get<Pagination<ProductListDTO>?>(URL, new KeyValuePair<string, object>("CategoryID", CategoryID),
                        new KeyValuePair<string, object>("page", pageSelected));
                }
            }
            else if (CategoryID == null)
            {
                response = await Get<Pagination<ProductListDTO>?>(URL, new KeyValuePair<string, object>("name", name),
                    new KeyValuePair<string, object>("page", pageSelected));
            }
            else
            {
                response = await Get<Pagination<ProductListDTO>?>(URL, new KeyValuePair<string, object>("name", name),
                    new KeyValuePair<string, object>("CategoryID", CategoryID), new KeyValuePair<string, object>("page", pageSelected));
            }

            return response;
        }

        public async Task<ResponseBase<Dictionary<string, object>?>> Index(string? name, int? CategoryID, int? page)
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
    }
}
