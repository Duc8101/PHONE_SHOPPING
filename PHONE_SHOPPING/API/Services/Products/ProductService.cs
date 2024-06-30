using API.Services.Base;
using AutoMapper;
using Common.Base;
using Common.Const;
using Common.DTO.ProductDTO;
using Common.Entity;
using Common.Pagination;
using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Services.Products
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IMapper mapper, PhoneShoppingContext context) : base(mapper, context)
        {

        }

        private IQueryable<Product> getQuery(string? name, int? CategoryID)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Category).Where(p => p.IsDeleted == false);
            if (name != null && name.Trim().Length > 0)
            {
                query = query.Where(p => p.ProductName.ToLower().Contains(name.Trim().ToLower()) || p.Category.Name.ToLower().Contains(name.Trim().ToLower()));
            }
            if (CategoryID.HasValue)
            {
                query = query.Where(p => p.CategoryId == CategoryID);
            }
            return query;
        }

        public ResponseBase List(bool isAdmin, string? name, int? CategoryID, int page)
        {
            int prePage = page - 1;
            int nextPage = page + 1;
            string preURL = isAdmin ? "/ManagerProduct" : "/Home";
            string nextURL = isAdmin ? "/ManagerProduct" : "/Home";
            string firstURL = isAdmin ? "/ManagerProduct" : "/Home";
            string lastURL = isAdmin ? "/ManagerProduct" : "/Home";
            try
            {
                IQueryable<Product> query = getQuery(name, CategoryID);
                int count = query.Count();
                int numberPage = (int)Math.Ceiling((double)count / PageSizeConst.MAX_PRODUCT_IN_PAGE);
                // if not choose category and name
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
                List<Product> listProduct = query.OrderByDescending(p => p.UpdateAt).Skip(PageSizeConst.MAX_PRODUCT_IN_PAGE * (page - 1))
                    .Take(PageSizeConst.MAX_PRODUCT_IN_PAGE).ToList();
                List<ProductListDTO> productDTOs = _mapper.Map<List<ProductListDTO>>(listProduct);
                Pagination<ProductListDTO> result = new Pagination<ProductListDTO>()
                {
                    PageSelected = page,
                    Results = productDTOs,
                    PRE_URL = preURL,
                    LAST_URL = lastURL,
                    NEXT_URL = nextURL,
                    FIRST_URL = firstURL,
                    NumberPage = numberPage,
                };
                return new ResponseBase(result, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Create(ProductCreateUpdateDTO DTO)
        {
            try
            {
                if (DTO.ProductName.Trim().Length == 0)
                {
                    return new ResponseBase(false, "You have to input product name", (int)HttpStatusCode.Conflict);
                }
                if (DTO.Image.Trim().Length == 0)
                {
                    return new ResponseBase(false, "You have to input image link", (int)HttpStatusCode.Conflict);
                }
                if (_context.Products.Any(p => p.ProductName == DTO.ProductName.Trim() && p.IsDeleted == false))
                {
                    return new ResponseBase(false, "Product existed", (int)HttpStatusCode.Conflict);
                }
                Product product = _mapper.Map<Product>(DTO);
                product.ProductId = Guid.NewGuid();
                product.CreatedAt = DateTime.Now;
                product.UpdateAt = DateTime.Now;
                product.IsDeleted = false;
                _context.Products.Add(product);
                _context.SaveChanges();
                return new ResponseBase(true, "Create successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Detail(Guid ProductID)
        {
            try
            {
                Product? product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == ProductID && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseBase("Not found product", (int)HttpStatusCode.NotFound);
                }
                ProductListDTO DTO = _mapper.Map<ProductListDTO>(product);
                return new ResponseBase(DTO, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Update(Guid ProductID, ProductCreateUpdateDTO DTO)
        {
            try
            {
                Product? product = _context.Products.Include(p => p.Category).Include(p => p.OrderDetails)
                    .FirstOrDefault(p => p.ProductId == ProductID && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseBase("Not found product", (int)HttpStatusCode.NotFound);
                }
                List<OrderDetail> list = product.OrderDetails.ToList();
                ProductListDTO data = _mapper.Map<ProductListDTO>(product);
                if (list.Count > 0)
                {
                    return new ResponseBase(data, "You can't update this product because it is being or has been ordered", (int)HttpStatusCode.Conflict);
                }
                product.ProductName = DTO.ProductName.Trim();
                product.Image = DTO.Image.Trim();
                product.Price = DTO.Price;
                product.CategoryId = DTO.CategoryId;
                product.Quantity = DTO.Quantity;
                data = _mapper.Map<ProductListDTO>(product);
                if (DTO.ProductName.Trim().Length == 0)
                {
                    return new ResponseBase(data, "You have to input product name", (int)HttpStatusCode.Conflict);
                }
                if (DTO.Image.Trim().Length == 0)
                {
                    return new ResponseBase(data, "You have to input image link", (int)HttpStatusCode.Conflict);
                }
                if (_context.Products.Any(p => p.ProductName == DTO.ProductName.Trim() && p.IsDeleted == false && p.ProductId != ProductID))
                {
                    return new ResponseBase(data, "Product existed", (int)HttpStatusCode.Conflict);
                }
                product.UpdateAt = DateTime.Now;
                _context.Products.Update(product);
                _context.SaveChanges();
                return new ResponseBase(data, "Update successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Delete(Guid ProductID)
        {
            try
            {
                Product? product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == ProductID && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseBase(false, "Not found product", (int)HttpStatusCode.NotFound);
                }
                product.IsDeleted = true;
                _context.Products.Update(product);
                _context.SaveChanges();
                return new ResponseBase(true, "Delete successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
