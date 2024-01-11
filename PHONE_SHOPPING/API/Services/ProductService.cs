using AutoMapper;
using DataAccess.DTO.ProductDTO;
using DataAccess.Entity;
using DataAccess.Model.DAO;

namespace API.Services
{
    public class ProductService : BaseService
    {
        private readonly DAOProduct daoProduct = new DAOProduct();

        public ProductService(IMapper mapper) : base(mapper)
        {

        }

        public async Task<List<ProductListDTO>> List()
        {
            List<Product> list = await daoProduct.getList();
            List<ProductListDTO> result = mapper.Map<List<ProductListDTO>>(list);
            return result;
        }
    }
}
