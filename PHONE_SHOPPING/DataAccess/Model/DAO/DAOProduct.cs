using DataAccess.Const;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model.DAO
{
    public class DAOProduct : BaseDAO
    {
        private IQueryable<Product> getQuery(int? CategoryID)
        {
            IQueryable<Product> query = context.Products.Include(p => p.Category).Where(p => p.IsDeleted == false);
            if (CategoryID != null)
            {
                query = query.Where(p => p.CategoryId == CategoryID);
            }
            return query;
        }
        public async Task<List<Product>> getList(int? CategoryID, int page)
        {
            IQueryable<Product> query = getQuery(CategoryID);
            return await query.OrderByDescending(p => p.UpdateAt).Skip(PageSizeConst.MAX_PRODUCT_IN_PAGE * (page - 1))
                .Take(PageSizeConst.MAX_PRODUCT_IN_PAGE).ToListAsync();
        }
        public async Task<int> getNumberPage(int? CategoryID)
        {
            IQueryable<Product> query = getQuery(CategoryID);
            int count = await query.CountAsync();
            return (int)Math.Ceiling((double)count / PageSizeConst.MAX_PRODUCT_IN_PAGE);
        }
    }
}
