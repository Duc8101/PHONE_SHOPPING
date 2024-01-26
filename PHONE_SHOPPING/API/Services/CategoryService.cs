using AutoMapper;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.Entity;
using DataAccess.Model.DAO;
using System.Net;

namespace API.Services
{
    public class CategoryService : BaseService
    {
        private readonly DAOCategory daoCategory = new DAOCategory();
        public CategoryService(IMapper mapper) : base(mapper)
        {

        }

        public async Task<ResponseDTO<List<CategoryListDTO>?>> ListAll()
        {
            try
            {
                List<Category> list = await daoCategory.getList();
                List<CategoryListDTO> result = mapper.Map<List<CategoryListDTO>>(list);
                return new ResponseDTO<List<CategoryListDTO>?>(result, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<CategoryListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO<PagedResultDTO<CategoryListDTO>?>> ListPaged(string? name, int page)
        {
            try
            {
                List<Category> list = await daoCategory.getList(name, page);
                List<CategoryListDTO> result = mapper.Map<List<CategoryListDTO>>(list);
                int number = await daoCategory.getNumberPage(name);
                string preURL = "/ManagerCategory";
                string nextURL = "/ManagerCategory";
                string firstURL = "/ManagerCategory";
                string lastURL = "/ManagerCategory";
                if (name == null || name.Trim().Length == 0)
                {
                    preURL = preURL + "?page=" + (page - 1);
                    nextURL = nextURL + "?page=" + (page + 1);
                    lastURL = lastURL + "?page=" + number;
                }
                else
                {
                    preURL = preURL + "?name=" + name.Trim() + "&page=" + (page - 1);
                    nextURL = nextURL + "?name=" + name.Trim() + "&page=" + (page + 1);
                    firstURL = firstURL + "?name=" + name.Trim();
                    lastURL = lastURL + "?name=" + name.Trim() + "&page=" + number;
                }
                PagedResultDTO<CategoryListDTO> data = new PagedResultDTO<CategoryListDTO>()
                {
                    PageSelected = page,
                    NEXT_URL = nextURL,
                    FIRST_URL = firstURL,
                    PRE_URL = preURL,
                    LAST_URL = lastURL,
                    NumberPage = number,
                    Results = result
                };
                return new ResponseDTO<PagedResultDTO<CategoryListDTO>?>(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<PagedResultDTO<CategoryListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO<bool>> Create(CategoryCreateUpdateDTO DTO)
        {
            try
            {
                if (await daoCategory.isExist(DTO.Name.Trim()))
                {
                    return new ResponseDTO<bool>(false, "Category existed", (int)HttpStatusCode.Conflict);
                }
                Category category = new Category()
                {
                    Name = DTO.Name.Trim(),
                    CreatedAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    IsDeleted = false,
                };
                await daoCategory.CreateCategory(category);
                return new ResponseDTO<bool>(true, "Create successful");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO<CategoryListDTO?>> Detail(int ID)
        {
            try
            {
                Category? category = await daoCategory.getCategory(ID);
                if (category == null)
                {
                    return new ResponseDTO<CategoryListDTO?>(null, "Not found category", (int)HttpStatusCode.NotFound);
                }
                CategoryListDTO data = mapper.Map<CategoryListDTO>(category);
                return new ResponseDTO<CategoryListDTO?>(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<CategoryListDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO<CategoryListDTO?>> Update(int ID, CategoryCreateUpdateDTO DTO)
        {
            try
            {
                Category? category = await daoCategory.getCategory(ID);
                if (category == null)
                {
                    return new ResponseDTO<CategoryListDTO?>(null, "Not found category", (int)HttpStatusCode.NotFound);
                }
                category.Name = DTO.Name.Trim();
                CategoryListDTO data = mapper.Map<CategoryListDTO>(category);
                if (await daoCategory.isExist(DTO.Name.Trim(), ID))
                {
                    return new ResponseDTO<CategoryListDTO?>(data, "Category existed", (int)HttpStatusCode.Conflict);
                }
                category.UpdateAt = DateTime.Now;
                await daoCategory.UpdateCategory(category);
                return new ResponseDTO<CategoryListDTO?>(data, "Update successful");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<CategoryListDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
