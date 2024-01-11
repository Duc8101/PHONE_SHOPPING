using AutoMapper;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
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
    }
}
