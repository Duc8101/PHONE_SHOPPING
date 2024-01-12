using AutoMapper;
using DataAccess.DTO.UserDTO;
using DataAccess.DTO;
using DataAccess.Model.DAO;
using DataAccess.Entity;
using System.Net;

namespace API.Services
{
    public class UserService : BaseService
    {
        private readonly DAOUser daoUser = new DAOUser();
        public UserService(IMapper mapper) : base(mapper)
        {

        }
        public async Task<ResponseDTO<UserDTO?>> Detail(Guid UserID)
        {
            try
            {
                User? user = await daoUser.getUser(UserID);
                if (user == null)
                {
                    return new ResponseDTO<UserDTO?>(null, "Not found user", (int) HttpStatusCode.NotFound);
                }
                UserDTO DTO = mapper.Map<UserDTO>(user);
                return new ResponseDTO<UserDTO?>(DTO, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDTO?>(null, ex.Message + " " + ex, (int) HttpStatusCode.InternalServerError);
            }
        }
    }
}
