using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model.DAO
{
    public class DAOProduct : BaseDAO
    {
        public async Task<List<Product>> getList()
        {
            return await context.Products.Include(p => p.Category).ToListAsync();
        }
    }
}
