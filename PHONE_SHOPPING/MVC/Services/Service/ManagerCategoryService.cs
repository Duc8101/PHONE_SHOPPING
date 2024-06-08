using Common.Base;
using Common.DTO.CategoryDTO;
using Common.Pagination;
using MVC.Services.IService;

namespace MVC.Services.Service
{
    public class ManagerCategoryService : BaseService, IManagerCategoryService
    {
        public ManagerCategoryService(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseBase<Pagination<CategoryListDTO>?>> Index(string? name, int? page)
        {
            int pageSelected = page == null ? 1 : page.Value;
            string URL = "https://localhost:7178/Category/List/Paged";
            ResponseBase<Pagination<CategoryListDTO>?> response;
            if (name == null || name.Trim().Length == 0)
            {
                response = await Get<Pagination<CategoryListDTO>?>(URL, new KeyValuePair<string, object>("page", pageSelected));
            }
            else
            {
                response = await Get<Pagination<CategoryListDTO>?>(URL, new KeyValuePair<string, object>("name", name.Trim()), new KeyValuePair<string, object>("page", pageSelected));
            }
            return response;

        }
        public async Task<ResponseBase<bool>> Create(CategoryCreateUpdateDTO DTO)
        {
            string URL = "https://localhost:7178/Category/Create";
            return await Post<CategoryCreateUpdateDTO, bool>(URL, DTO);
        }
        public async Task<ResponseBase<CategoryListDTO?>> Update(int ID)
        {
            string URL = "https://localhost:7178/Category/Detail/" + ID;
            return await Get<CategoryListDTO?>(URL);
        }
        public async Task<ResponseBase<CategoryListDTO?>> Update(int ID, CategoryCreateUpdateDTO DTO)
        {
            string URL = "https://localhost:7178/Category/Update/" + ID;
            return await Put<CategoryCreateUpdateDTO, CategoryListDTO?>(URL, DTO);
        }

    }
}
