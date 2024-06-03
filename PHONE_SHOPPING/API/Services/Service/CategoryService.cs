using API.Services.IService;
using AutoMapper;
using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.Entity;
using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Services.Service
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IMapper mapper, PHONE_SHOPPINGContext context) : base(mapper, context)
        {

        }

        public async Task<ResponseDTO> ListAll()
        {
            try
            {
                List<Category> list = await _context.Categories.ToListAsync();
                List<CategoryListDTO> result = _mapper.Map<List<CategoryListDTO>>(list);
                return new ResponseDTO(result, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
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
        public async Task<ResponseDTO> ListPaged(string? name, int page)
        {
            try
            {
                IQueryable<Category> query = getQuery(name);
                List<Category> list = await query.Skip(PageSizeConst.MAX_CATEGORY_IN_PAGE * (page - 1)).Take(PageSizeConst.MAX_CATEGORY_IN_PAGE)
                    .OrderByDescending(c => c.UpdateAt).ToListAsync();
                List<CategoryListDTO> result = _mapper.Map<List<CategoryListDTO>>(list);
                int count = await query.CountAsync();
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
                return new ResponseDTO(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO> Create(CategoryCreateUpdateDTO DTO)
        {
            try
            {
                if (await _context.Categories.AnyAsync(c => c.Name == DTO.Name.Trim()))
                {
                    return new ResponseDTO(false, "Category existed", (int)HttpStatusCode.Conflict);
                }
                Category category = new Category()
                {
                    Name = DTO.Name.Trim(),
                    CreatedAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    IsDeleted = false,
                };
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return new ResponseDTO(true, "Create successful");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ResponseDTO> Detail(int ID)
        {
            try
            {
                Category? category = await _context.Categories.FindAsync(ID);
                if (category == null)
                {
                    return new ResponseDTO(null, "Not found category", (int)HttpStatusCode.NotFound);
                }
                CategoryListDTO data = _mapper.Map<CategoryListDTO>(category);
                return new ResponseDTO(data, string.Empty);
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
                Category? category = await _context.Categories.FindAsync(ID);
                if (category == null)
                {
                    return new ResponseDTO(null, "Not found category", (int)HttpStatusCode.NotFound);
                }
                category.Name = DTO.Name.Trim();
                CategoryListDTO data = _mapper.Map<CategoryListDTO>(category);
                if (await _context.Categories.AnyAsync(c => c.Name == DTO.Name.Trim() && c.Id != ID))
                {
                    return new ResponseDTO(data, "Category existed", (int)HttpStatusCode.Conflict);
                }
                category.UpdateAt = DateTime.Now;
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return new ResponseDTO(data, "Update successful");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
