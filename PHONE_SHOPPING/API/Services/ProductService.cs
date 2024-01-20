using AutoMapper;
using DataAccess.DTO;
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

        public async Task<ResponseDTO<PagedResultDTO<ProductListDTO>?>> List(bool isAdmin, string? name, int? CategoryID, int page)
        {
            int prePage = page - 1;
            int nextPage = page + 1;
            string preURL = isAdmin ? "/ManagerProduct" : "/Home";
            string nextURL = isAdmin ? "/ManagerProduct" : "/Home";
            string firstURL = isAdmin ? "/ManagerProduct" : "/Home";
            string lastURL = isAdmin ? "/ManagerProduct" : "/Home";
            try
            {
                int numberPage = await daoProduct.getNumberPage(name, CategoryID);
                // if not choose category
                if (CategoryID == null)
                {
                    preURL = preURL + "?page=" + prePage;
                    nextURL = nextURL + "?page=" + nextPage;
                    lastURL = lastURL + "?page=" + numberPage;
                }
                else
                {
                    preURL = preURL + "?CategoryID=" + CategoryID + "&page=" + prePage;
                    nextURL = nextURL + "?CategoryID=" + CategoryID + "&page=" + nextPage;
                    firstURL = firstURL + "?CategoryID=" + CategoryID;
                    lastURL = lastURL + "?CategoryID=" + CategoryID + "&page=" + numberPage;
                }
                List<Product> listProduct = await daoProduct.getList(name, CategoryID, page);
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
                return new ResponseDTO<PagedResultDTO<ProductListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
