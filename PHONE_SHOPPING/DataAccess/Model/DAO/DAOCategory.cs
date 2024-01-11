using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model.DAO
{
    public class DAOCategory : BaseDAO
    {
        public async Task<List<Category>> getList()
        {
            return await context.Categories.ToListAsync();
        }
    }
}
