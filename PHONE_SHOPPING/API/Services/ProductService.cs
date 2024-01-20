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
        private readonly DAOCategory daoCategory = new DAOCategory();
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
                if (CategoryID == null && (name == null || name.Trim().Length == 0))
                {
                    preURL = preURL + "?page=" + prePage;
                    nextURL = nextURL + "?page=" + nextPage;
                    lastURL = lastURL + "?page=" + numberPage;
                }
                else
                {
                    if (name == null || name.Trim().Length == 0)
                    {
                        preURL = preURL + "?CategoryID=" + CategoryID + "&page=" + prePage;
                        nextURL = nextURL + "?CategoryID=" + CategoryID + "&page=" + nextPage;
                        firstURL = firstURL + "?CategoryID=" + CategoryID;
                        lastURL = lastURL + "?CategoryID=" + CategoryID + "&page=" + numberPage;
                    }
                    else if (CategoryID == null)
                    {
                        preURL = preURL + "?name=" + name.Trim() + "&page=" + prePage;
                        nextURL = nextURL + "?name=" + name.Trim() + "&page=" + nextPage;
                        firstURL = firstURL + "?name=" + name.Trim();
                        lastURL = lastURL + "?name=" + name.Trim() + "&page=" + numberPage;
                    }
                    else
                    {
                        preURL = preURL + "?name=" + name.Trim() + "&CategoryID=" + CategoryID + "&page=" + prePage;
                        nextURL = nextURL + "?name=" + name.Trim() + "&CategoryID=" + CategoryID + "&page=" + nextPage;
                        firstURL = firstURL + "?name=" + name.Trim() + "&CategoryID=" + CategoryID;
                        lastURL = lastURL + "?name=" + name.Trim() + "&CategoryID=" + CategoryID + "&page=" + numberPage;
                    }
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

        public async Task<ResponseDTO<List<CategoryListDTO>?>> Create(ProductCreateUpdateDTO DTO)
        {
            try
            {
                List<Category> list = await daoCategory.getList();
                List<CategoryListDTO> data = mapper.Map<List<CategoryListDTO>>(list);
                if (DTO.ProductName.Trim().Length == 0)
                {
                    return new ResponseDTO<List<CategoryListDTO>?>(data, "You have to input product name", (int)HttpStatusCode.Conflict);
                }
                if (DTO.Image.Trim().Length == 0)
                {
                    return new ResponseDTO<List<CategoryListDTO>?>(data, "You have to input image link", (int)HttpStatusCode.Conflict);
                }
                Product product = mapper.Map<Product>(DTO);
                product.ProductId = Guid.NewGuid();
                product.CreatedAt = DateTime.Now;
                product.UpdateAt = DateTime.Now;
                product.IsDeleted = false;
                await daoProduct.CreateProduct(product);
                return new ResponseDTO<List<CategoryListDTO>?>(data, "Create successful");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<CategoryListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
