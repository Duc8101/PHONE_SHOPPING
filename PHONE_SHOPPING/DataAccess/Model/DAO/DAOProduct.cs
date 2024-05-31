using DataAccess.Const;
using DataAccess.Entity;
using DataAccess.Model.IDAO;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model.DAO
{
    public class DAOProduct : BaseDAO, IDAOProduct
    {
        public DAOProduct(PHONE_SHOPPINGContext context) : base(context)
        {
        }

        private IQueryable<Product> getQuery(string? name, int? CategoryID)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Category).Where(p => p.IsDeleted == false);
            if(name != null && name.Trim().Length > 0)
            {
                query = query.Where(p => p.ProductName.ToLower().Contains(name.Trim().ToLower()) || p.Category.Name.ToLower().Contains(name.Trim().ToLower()));
            }
            if (CategoryID.HasValue)
            {
                query = query.Where(p => p.CategoryId == CategoryID);
            }
            return query;
        }
        public async Task<List<Product>> getList(string? name, int? CategoryID, int page)
        {
            IQueryable<Product> query = getQuery(name,CategoryID);
            return await query.OrderByDescending(p => p.UpdateAt).Skip(PageSizeConst.MAX_PRODUCT_IN_PAGE * (page - 1))
                .Take(PageSizeConst.MAX_PRODUCT_IN_PAGE).ToListAsync();
        }
        public async Task<int> getNumberPage(string? name, int? CategoryID)
        {
            IQueryable<Product> query = getQuery(name, CategoryID);
            int count = await query.CountAsync();
            return (int)Math.Ceiling((double)count / PageSizeConst.MAX_PRODUCT_IN_PAGE);
        }
        public async Task<Product?> getProduct(Guid ProductID)
        {
            return await _context.Products.Include(p => p.Category).SingleOrDefaultAsync(p => p.ProductId == ProductID && p.IsDeleted == false);
        }
        public async Task<bool> isExist(string ProductName)
        {
            return await _context.Products.AnyAsync(p => p.ProductName == ProductName.Trim() && p.IsDeleted == false);
        }
        public async Task CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> isExist(string ProductName, Guid ProductID)
        {
            return await _context.Products.AnyAsync(p => p.ProductName == ProductName.Trim() && p.IsDeleted == false && p.ProductId != ProductID);
        }
        public async Task UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProduct(Product product)
        {
            product.IsDeleted = true;
            await UpdateProduct(product);
        }
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

    }
}
