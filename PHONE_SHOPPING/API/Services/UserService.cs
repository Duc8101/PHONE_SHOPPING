using AutoMapper;
using DataAccess.DTO;
using DataAccess.DTO.UserDTO;
using DataAccess.Entity;
using DataAccess.Model.DAO;
using System.Net;

namespace API.Services
{
    public class UserService : BaseService
    {
        private readonly DAOUser daoUser = new DAOUser();
        private readonly DAOCart daoCart = new DAOCart();
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
                    return new ResponseDTO<UserDTO?>(null, "Not found user", (int)HttpStatusCode.NotFound);
                }
                UserDTO DTO = mapper.Map<UserDTO>(user);
                return new ResponseDTO<UserDTO?>(DTO, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<UserDTO?>> Login(LoginDTO DTO)
        {
            try
            {
                User? user = await daoUser.getUser(DTO);
                if (user == null)
                {
                    return new ResponseDTO<UserDTO?>(null, "Username or password incorrect", (int)HttpStatusCode.Conflict);
                }
                UserDTO data = mapper.Map<UserDTO>(user);
                return new ResponseDTO<UserDTO?>(data, string.Empty);

            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<bool>> Logout(Guid UserID)
        {
            try
            {
                User? user = await daoUser.getUser(UserID);
                if (user == null)
                {
                    return new ResponseDTO<bool>(false, "Not found user", (int)HttpStatusCode.NotFound);
                }
                List<Cart> list = await daoCart.getList(UserID);
                foreach (Cart cart in list)
                {
                    await daoCart.DeleteCart(cart);
                }
                return new ResponseDTO<bool>(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
