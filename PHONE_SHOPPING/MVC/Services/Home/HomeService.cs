using Common.Base;
using Common.DTO.CategoryDTO;
using Common.DTO.ProductDTO;
using Common.Paginations;
using MVC.Services.Base;

namespace MVC.Services.Home
{
    public class HomeService : BaseService, IHomeService
    {

        private async Task<ResponseBase<List<CategoryListDTO>?>> getListCategory()
        {
            string URL = "https://localhost:7077/Category/List/All";
            return await Get<List<CategoryListDTO>?>(URL);
        }

        private async Task<ResponseBase<Pagination<ProductListDTO>?>> getPagination(string? name, int? categoryId, int? page)
        {
            int pageSelected = page == null ? 1 : page.Value;
            string URL = "https://localhost:7077/Product/Home/List";
            ResponseBase<Pagination<ProductListDTO>?> response;
            if (name == null)
            {
                if (categoryId == null)
                {
                    response = await Get<Pagination<ProductListDTO>?>(URL, new KeyValuePair<string, object>("page", pageSelected));
                }
                else
                {
                    response = await Get<Pagination<ProductListDTO>?>(URL, new KeyValuePair<string, object>("categoryId", categoryId),
                        new KeyValuePair<string, object>("page", pageSelected));
                }
            }
            else if (categoryId == null)
            {
                response = await Get<Pagination<ProductListDTO>?>(URL, new KeyValuePair<string, object>("name", name),
                    new KeyValuePair<string, object>("page", pageSelected));
            }
            else
            {
                response = await Get<Pagination<ProductListDTO>?>(URL, new KeyValuePair<string, object>("name", name),
                    new KeyValuePair<string, object>("categoryId", categoryId), new KeyValuePair<string, object>("page", pageSelected));
            }

            return response;
        }

        public async Task<ResponseBase<Dictionary<string, object>?>> Index(string? name, int? categoryId, int? page)
        {
            ResponseBase<List<CategoryListDTO>?> resCategory = await getListCategory();
            ResponseBase<Pagination<ProductListDTO>?> resProduct = await getPagination(name, categoryId, page);
            if (resCategory.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, resCategory.Message, resCategory.Code);
            }
            if (resProduct.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, resProduct.Message, resProduct.Code);
            }
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"result",  resProduct.Data},
                {"list",  resCategory.Data},
                {"categoryId",  categoryId == null ? 0 : categoryId},
                {"name",  name == null ? "" : name.Trim()},
            };
            return new ResponseBase<Dictionary<string, object>?>(data);
        }
    }
}
