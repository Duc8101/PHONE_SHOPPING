using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model.DAO
{
    public class DAOUser : BaseDAO
    {
        public async Task<User?> getUser(Guid UserID)
        {
            return await context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.UserId == UserID);
        }
    }
}
