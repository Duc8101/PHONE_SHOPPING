using DataAccess.Const;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model.DAO
{
    public class DAOCategory : PHONE_SHOPPINGContext
    {
        public async Task<List<Category>> getList()
        {
            return await Categories.ToListAsync();
        }

        private IQueryable<Category> getQuery(string? name)
        {
            IQueryable<Category> query = Categories;
            if(name != null && name.Trim().Length > 0)
            {
                query = query.Where(c => c.Name.ToLower().Contains(name.Trim().ToLower()));
            }
            return query;
        }
        public async Task<List<Category>> getList(string? name, int page)
        {
            IQueryable<Category> query = getQuery(name);
            return await query.Skip(PageSizeConst.MAX_CATEGORY_IN_PAGE * (page - 1)).Take(PageSizeConst.MAX_CATEGORY_IN_PAGE).OrderByDescending(c => c.UpdateAt).ToListAsync();
        }
        public async Task<int> getNumberPage(string? name)
        {
            IQueryable<Category> query = getQuery(name);
            int count = await query.CountAsync();
            return (int)Math.Ceiling((double)count / PageSizeConst.MAX_CATEGORY_IN_PAGE);
        }

        public async Task<bool> isExist(string name)
        {
            return await Categories.AnyAsync(c => c.Name == name.Trim());
        }

        public async Task CreateCategory(Category category)
        {
            await Categories.AddAsync(category);
            await SaveChangesAsync();
        }

        public async Task<Category?> getCategory(int ID)
        {
            return await Categories.SingleOrDefaultAsync(c => c.Id == ID);
        }

        public async Task<bool> isExist(string name, int ID)
        {
            return await Categories.AnyAsync(c => c.Name == name.Trim() && c.Id != ID);
        }

        public async Task UpdateCategory(Category category)
        {
            Categories.Update(category);
            await SaveChangesAsync();
        }
    }
}
