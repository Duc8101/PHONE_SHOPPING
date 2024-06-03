﻿using API.Services.IService;
using AutoMapper;
using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
using DataAccess.Entity;
using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Services.Service
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IMapper mapper, PHONE_SHOPPINGContext context) : base(mapper, context)
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
                IQueryable<Product> query = getQuery(name, CategoryID);
                int count = await query.CountAsync();
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
                List<Product> listProduct = await query.OrderByDescending(p => p.UpdateAt).Skip(PageSizeConst.MAX_PRODUCT_IN_PAGE * (page - 1))
                    .Take(PageSizeConst.MAX_PRODUCT_IN_PAGE).ToListAsync();
                List<ProductListDTO> productDTOs = _mapper.Map<List<ProductListDTO>>(listProduct);
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

        public async Task<ResponseDTO<bool>> Create(ProductCreateUpdateDTO DTO)
        {
            try
            {
                List<Category> list = await _context.Categories.ToListAsync();
                List<CategoryListDTO> data = _mapper.Map<List<CategoryListDTO>>(list);
                if (DTO.ProductName.Trim().Length == 0)
                {
                    return new ResponseDTO<bool>(false, "You have to input product name", (int)HttpStatusCode.Conflict);
                }
                if (DTO.Image.Trim().Length == 0)
                {
                    return new ResponseDTO<bool>(false, "You have to input image link", (int)HttpStatusCode.Conflict);
                }
                if (await _context.Products.AnyAsync(p => p.ProductName == DTO.ProductName.Trim() && p.IsDeleted == false))
                {
                    return new ResponseDTO<bool>(false, "Product existed", (int)HttpStatusCode.Conflict);
                }
                Product product = _mapper.Map<Product>(DTO);
                product.ProductId = Guid.NewGuid();
                product.CreatedAt = DateTime.Now;
                product.UpdateAt = DateTime.Now;
                product.IsDeleted = false;
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return new ResponseDTO<bool>(true, "Create successful");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<ProductListDTO?>> Detail(Guid ProductID)
        {
            try
            {
                Product? product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == ProductID && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseDTO<ProductListDTO?>(null, "Not found product", (int)HttpStatusCode.NotFound);
                }
                ProductListDTO DTO = _mapper.Map<ProductListDTO>(product);
                return new ResponseDTO<ProductListDTO?>(DTO, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<ProductListDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<ProductListDTO?>> Update(Guid ProductID, ProductCreateUpdateDTO DTO)
        {
            try
            {
                Product? product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == ProductID && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseDTO<ProductListDTO?>(null, "Not found product", (int)HttpStatusCode.NotFound);
                }
                product.ProductName = DTO.ProductName.Trim();
                product.Image = DTO.Image.Trim();
                product.Price = DTO.Price;
                product.CategoryId = DTO.CategoryId;
                product.Quantity = DTO.Quantity;
                ProductListDTO data = _mapper.Map<ProductListDTO>(product);
                if (DTO.ProductName.Trim().Length == 0)
                {
                    return new ResponseDTO<ProductListDTO?>(data, "You have to input product name", (int)HttpStatusCode.Conflict);
                }
                if (DTO.Image.Trim().Length == 0)
                {
                    return new ResponseDTO<ProductListDTO?>(data, "You have to input image link", (int)HttpStatusCode.Conflict);
                }
                if (await _context.Products.AnyAsync(p => p.ProductName == DTO.ProductName.Trim() && p.IsDeleted == false))
                {
                    return new ResponseDTO<ProductListDTO?>(data, "Product existed", (int)HttpStatusCode.Conflict);
                }
                product.UpdateAt = DateTime.Now;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return new ResponseDTO<ProductListDTO?>(data, "Update successful");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<ProductListDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<PagedResultDTO<ProductListDTO>?>> Delete(Guid ProductID)
        {
            try
            {
                Product? product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == ProductID && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseDTO<PagedResultDTO<ProductListDTO>?>(null, "Not found product", (int)HttpStatusCode.NotFound);
                }
                product.IsDeleted = true;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                ResponseDTO<PagedResultDTO<ProductListDTO>?> result = await List(true, null, null, 1);
                if (result.Code == (int)HttpStatusCode.OK)
                {
                    return new ResponseDTO<PagedResultDTO<ProductListDTO>?>(result.Data, "Delete successful");
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO<PagedResultDTO<ProductListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}