using DataAccess.Const;
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
        public async Task<bool> isExist(string username, string email)
        {
            return await context.Users.AnyAsync(u => u.Username == username || u.Email == email.Trim());
        }
        public async Task CreateUser(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }
        public async Task<User?> getUser(string email)
        {
            return await context.Users.SingleOrDefaultAsync(u => u.Email == email.Trim());
        }
        public async Task UpdateUser(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
        public async Task<bool> isExist(string email , Guid UserID)
        {
            return await context.Users.AnyAsync(u => u.Email == email.Trim() && u.UserId != UserID);
        }
        public async Task<List<string>> getEmailAdmins()
        {
            return await context.Users.Where(u => u.RoleId == RoleConst.ROLE_ADMIN).Select(u => u.Email).ToListAsync();
        }

    }
}
