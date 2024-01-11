using AutoMapper;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
using DataAccess.Entity;
using DataAccess.Model.DAO;
using System.Net;

namespace API.Services
{
    public class ProductService : BaseService
    {
        private readonly DAOProduct daoProduct = new DAOProduct();
        public ProductService(IMapper mapper) : base(mapper)
        {

        }

        public async Task<ResponseDTO<PagedResultDTO<ProductListDTO>?>> List(int? CategoryID, int page)
        {
            int prePage = page - 1;
            int nextPage = page + 1;
            string preURL;
            string nextURL;
            string firstURL;
            string lastURL;
            try
            {
                int numberPage = await daoProduct.getNumberPage(CategoryID);
                // if not choose category
                if (CategoryID == null)
                {
                    preURL = "/Home" + "?page=" + prePage;
                    nextURL = "/Home" + "?page=" + nextPage;
                    firstURL = "/Home";
                    lastURL = "/Home" + "?page=" + numberPage;
                }
                else
                {
                    preURL = "/Home" + "?CategoryID=" + CategoryID + "&page=" + prePage;
                    nextURL = "/Home" + "?CategoryID=" + CategoryID + "&page=" + nextPage;
                    firstURL = "/Home" + "?CategoryID=" + CategoryID;
                    lastURL = "/Home" + "?CategoryID=" + CategoryID + "&page=" + numberPage;
                }
                List<Product> listProduct = await daoProduct.getList(CategoryID, page);
                List<ProductListDTO> productDTOs = mapper.Map<List<ProductListDTO>>(listProduct);
                PagedResultDTO<ProductListDTO> result = new PagedResultDTO<ProductListDTO>()
                {
                    PageSelected = page,
                    Results = productDTOs,
                    PRE_URL = preURL,
                    LAST_URL = lastURL,
                    NEXT_URL = nextURL,
                    FIRST_URL = firstURL,
                    NumberPage = numberPage,
                };       
                return new ResponseDTO<PagedResultDTO<ProductListDTO>?>(result, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<PagedResultDTO<ProductListDTO>?>(null, ex.Message + " " + ex, (int) HttpStatusCode.InternalServerError);
            }
        }
    }
}
