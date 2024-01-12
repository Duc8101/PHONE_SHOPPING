using DataAccess.DTO.UserDTO;
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
        public async Task<User?> getUser(LoginDTO DTO)
        {
            User? user = await context.Users.SingleOrDefaultAsync(u => u.Username == DTO.Username && u.IsDeleted == false);
            if (user == null || string.Compare(user.Password, UserUtil.HashPassword(DTO.Password), false) != 0)
            {
                return null;
            }
            return user;
        }
    }
}
