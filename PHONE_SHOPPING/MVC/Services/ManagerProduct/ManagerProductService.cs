using Common.Base;
using Common.DTO.CategoryDTO;
using Common.DTO.ProductDTO;
using Common.Paginations;
using MVC.Services.Base;
using System.Net;

namespace MVC.Services.ManagerProduct
{
    public class ManagerProductService : BaseService, IManagerProductService
    {

        private async Task<ResponseBase<List<CategoryListDTO>?>> getListCategory()
        {
            string URL = "https://localhost:7077/Category/List/All";
            return await Get<List<CategoryListDTO>?>(URL);
        }

        private async Task<ResponseBase<Pagination<ProductListDTO>?>> getPagination(string? name, int? categoryId, int? page)
        {
            int pageSelected = page == null ? 1 : page.Value;
            string URL = "https://localhost:7077/Product/Manager/List";
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
                {"result", resProduct.Data},
                {"list", resCategory.Data},
                {"categoryId", categoryId == null ? 0 : categoryId},
                {"name", name == null ? "" : name.Trim()},
            };
            return new ResponseBase<Dictionary<string, object>?>(data);
        }

        public async Task<ResponseBase<List<CategoryListDTO>?>> Create()
        {
            return await getListCategory();
        }

        public async Task<ResponseBase<List<CategoryListDTO>?>> Create(ProductCreateUpdateDTO DTO)
        {
            DTO.ProductName = DTO.ProductName == null ? "" : DTO.ProductName.Trim();
            DTO.Image = DTO.Image == null ? "" : DTO.Image.Trim();
            string URL = "https://localhost:7077/Product/Create";
            ResponseBase<bool?> response = await Post<ProductCreateUpdateDTO, bool?>(URL, DTO);
            if ((response.Data == false && response.Code != (int)HttpStatusCode.Conflict) || response.Data == null)
            {
                return new ResponseBase<List<CategoryListDTO>?>(null, response.Message, response.Code);
            }
            ResponseBase<List<CategoryListDTO>?> result = await getListCategory();
            if (result.Data == null)
            {
                return new ResponseBase<List<CategoryListDTO>?>(null, result.Message, result.Code);
            }
            if (response.Code == (int)HttpStatusCode.OK || response.Code == (int)HttpStatusCode.Conflict)
            {
                return new ResponseBase<List<CategoryListDTO>?>(result.Data, response.Message, response.Code);
            }
            return new ResponseBase<List<CategoryListDTO>?>(null, response.Message, response.Code);
        }

        public async Task<ResponseBase<Dictionary<string, object>?>> Update(Guid productId)
        {
            string URL = "https://localhost:7077/Product/Detail/" + productId;
            ResponseBase<ProductListDTO?> resPro = await Get<ProductListDTO?>(URL);
            ResponseBase<List<CategoryListDTO>?> resCat = await getListCategory();
            if (resCat.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, resCat.Message, resCat.Code);
            }
            if (resPro.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, resPro.Message, resPro.Code);
            }
            Dictionary<string, object> result = new Dictionary<string, object>()
            {
                {"product", resPro.Data},
                {"list", resCat.Data},
            };
            return new ResponseBase<Dictionary<string, object>?>(result);
        }

        public async Task<ResponseBase<Dictionary<string, object>?>> Update(Guid productId, ProductCreateUpdateDTO DTO)
        {
            DTO.ProductName = DTO.ProductName == null ? "" : DTO.ProductName.Trim();
            DTO.Image = DTO.Image == null ? "" : DTO.Image.Trim();
            string URL = "https://localhost:7077/Product/Update/" + productId;
            ResponseBase<ProductListDTO?>? resPro = await Put<ProductCreateUpdateDTO, ProductListDTO?>(URL, DTO);
            ResponseBase<List<CategoryListDTO>?> resCat = await getListCategory();
            if (resCat.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, resCat.Message, resCat.Code);
            }

            if (resPro.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, resPro.Message, resPro.Code);
            }
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"product", resPro.Data},
                {"list",  resCat.Data},
            };
            return new ResponseBase<Dictionary<string, object>?>(data, resPro.Message, resPro.Code);
        }

        public async Task<ResponseBase<Dictionary<string, object>?>> Delete(Guid productId)
        {
            string URL = "https://localhost:7077/Product/Delete/" + productId;
            ResponseBase<bool?> response = await Delete<bool?>(URL);
            if (response.Data == false || response.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, response.Message, response.Code);
            }
            ResponseBase<Dictionary<string, object>?> result = await Index(null, null, null);
            if (result.Code == (int)HttpStatusCode.OK)
            {
                result.Message = response.Message;
            }
            return result;
        }
    }
}
