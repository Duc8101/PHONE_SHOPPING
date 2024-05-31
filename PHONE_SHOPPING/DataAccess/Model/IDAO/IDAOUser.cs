using DataAccess.DTO.UserDTO;
using DataAccess.Entity;

namespace DataAccess.Model.IDAO
{
    public interface IDAOUser
    {
        Task<User?> getUser(Guid UserID);
        Task<User?> getUser(LoginDTO DTO);
        Task<bool> isExist(string username, string email);
        Task CreateUser(User user);
        Task<User?> getUser(string email);
        Task UpdateUser(User user);
        Task<bool> isExist(string email, Guid UserID);
        Task<List<string>> getEmailAdmins();
    }
}
