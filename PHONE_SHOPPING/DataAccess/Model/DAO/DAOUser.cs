using DataAccess.Const;
using DataAccess.DTO.UserDTO;
using DataAccess.Entity;
using DataAccess.Model.IDAO;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model.DAO
{
    public class DAOUser : BaseDAO, IDAOUser
    {
        public DAOUser(PHONE_SHOPPINGContext context) : base(context)
        {
        }

        public async Task<User?> getUser(Guid UserID)
        {
            return await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.UserId == UserID);
        }
        public async Task<User?> getUser(LoginDTO DTO)
        {
            User? user = await _context.Users.SingleOrDefaultAsync(u => u.Username == DTO.Username && u.IsDeleted == false);
            if (user == null || string.Compare(user.Password, UserUtil.HashPassword(DTO.Password), false) != 0)
            {
                return null;
            }
            return user;
        }
        public async Task<bool> isExist(string username, string email)
        {
            return await _context.Users.AnyAsync(u => u.Username == username || u.Email == email.Trim());
        }
        public async Task CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> getUser(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email.Trim());
        }
        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> isExist(string email , Guid UserID)
        {
            return await _context.Users.AnyAsync(u => u.Email == email.Trim() && u.UserId != UserID);
        }
        public async Task<List<string>> getEmailAdmins()
        {
            return await _context.Users.Where(u => u.RoleId == RoleConst.ROLE_ADMIN).Select(u => u.Email).ToListAsync();
        }

    }
}
