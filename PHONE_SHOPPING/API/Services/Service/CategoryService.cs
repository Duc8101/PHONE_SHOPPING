using API.Services.IService;
using AutoMapper;
using Common.Base;
using Common.Const;
using Common.DTO.CategoryDTO;
using Common.Entity;
using Common.Pagination;
using DataAccess.DBContext;
using System.Net;

namespace API.Services.Service
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IMapper mapper, PhoneShoppingContext context) : base(mapper, context)
        {

        }

        public ResponseBase<List<CategoryListDTO>?> ListAll()
        {
            try
            {
                List<Category> list = _context.Categories.ToList();
                List<CategoryListDTO> result = _mapper.Map<List<CategoryListDTO>>(list);
                return new ResponseBase<List<CategoryListDTO>?>(result, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<CategoryListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        private IQueryable<Category> getQuery(string? name)
        {
            IQueryable<Category> query = _context.Categories;
            if (name != null && name.Trim().Length > 0)
            {
                query = query.Where(c => c.Name.ToLower().Contains(name.Trim().ToLower()));
            }
            return query;
        }
        public ResponseBase<Pagination<CategoryListDTO>?> ListPaged(string? name, int page)
        {
            try
            {
                IQueryable<Category> query = getQuery(name);
                List<Category> list = query.Skip(PageSizeConst.MAX_CATEGORY_IN_PAGE * (page - 1)).Take(PageSizeConst.MAX_CATEGORY_IN_PAGE)
                    .OrderByDescending(c => c.UpdateAt).ToList();
                List<CategoryListDTO> result = _mapper.Map<List<CategoryListDTO>>(list);
                int count = query.Count();
                int number = (int)Math.Ceiling((double)count / PageSizeConst.MAX_CATEGORY_IN_PAGE);
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
                Pagination<CategoryListDTO> data = new Pagination<CategoryListDTO>()
                {
                    PageSelected = page,
                    NEXT_URL = nextURL,
                    FIRST_URL = firstURL,
                    PRE_URL = preURL,
                    LAST_URL = lastURL,
                    NumberPage = number,
                    Results = result
                };
                return new ResponseBase<Pagination<CategoryListDTO>?>(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase<Pagination<CategoryListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public ResponseBase<bool> Create(CategoryCreateUpdateDTO DTO)
        {
            try
            {
                if (_context.Categories.Any(c => c.Name == DTO.Name.Trim()))
                {
                    return new ResponseBase<bool>(false, "Category existed", (int)HttpStatusCode.Conflict);
                }
                Category category = new Category()
                {
                    Name = DTO.Name.Trim(),
                    CreatedAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    IsDeleted = false,
                };
                _context.Categories.Add(category);
                _context.SaveChanges();
                return new ResponseBase<bool>(true, "Create successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public ResponseBase<CategoryListDTO?> Detail(int ID)
        {
            try
            {
                Category? category = _context.Categories.Find(ID);
                if (category == null)
                {
                    return new ResponseBase<CategoryListDTO?>(null, "Not found category", (int)HttpStatusCode.NotFound);
                }
                CategoryListDTO data = _mapper.Map<CategoryListDTO>(category);
                return new ResponseBase<CategoryListDTO?>(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase<CategoryListDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public ResponseBase<CategoryListDTO?> Update(int ID, CategoryCreateUpdateDTO DTO)
        {
            try
            {
                Category? category = _context.Categories.Find(ID);
                if (category == null)
                {
                    return new ResponseBase<CategoryListDTO?>(null, "Not found category", (int)HttpStatusCode.NotFound);
                }
                category.Name = DTO.Name.Trim();
                CategoryListDTO data = _mapper.Map<CategoryListDTO>(category);
                if (_context.Categories.Any(c => c.Name == DTO.Name.Trim() && c.Id != ID))
                {
                    return new ResponseBase<CategoryListDTO?>(data, "Category existed", (int)HttpStatusCode.Conflict);
                }
                category.UpdateAt = DateTime.Now;
                _context.Categories.Update(category);
                _context.SaveChanges();
                return new ResponseBase<CategoryListDTO?>(data, "Update successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase<CategoryListDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
