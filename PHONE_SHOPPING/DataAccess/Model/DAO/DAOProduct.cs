using DataAccess.Const;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model.DAO
{
    public class DAOProduct : BaseDAO
    {
        private IQueryable<Product> getQuery(string? name, int? CategoryID)
        {
            IQueryable<Product> query = context.Products.Include(p => p.Category).Where(p => p.IsDeleted == false);
            if(name != null && name.Trim().Length > 0)
            {
                query = query.Where(p => p.ProductName.ToLower().Contains(name.Trim().ToLower()) || p.Category.Name.ToLower().Contains(name.Trim().ToLower()));
            }
            if (CategoryID != null)
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
            return await context.Products.SingleOrDefaultAsync(p => p.ProductId == ProductID && p.IsDeleted == false);
        }
    }
}
