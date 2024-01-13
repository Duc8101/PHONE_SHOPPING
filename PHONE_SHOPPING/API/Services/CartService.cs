using AutoMapper;
using DataAccess.DTO;
using DataAccess.DTO.CartDTO;
using DataAccess.Entity;
using DataAccess.Model.DAO;
using System.Net;

namespace API.Services
{
    public class CartService : BaseService
    {
        private readonly DAOUser daoUser = new DAOUser();
        private readonly DAOCart daoCart = new DAOCart();
        private readonly DAOProduct daoProduct = new DAOProduct();
        public CartService(IMapper mapper) : base(mapper)
        {

        }

        public async Task<ResponseDTO<List<CartListDTO>?>> List(Guid UserID)
        {
            try
            {
                User? user = await daoUser.getUser(UserID);
                if (user == null)
                {
                    return new ResponseDTO<List<CartListDTO>?>(null, "Not found user", (int)HttpStatusCode.NotFound);
                }
                List<Cart> list = await daoCart.getList(UserID);
                List<CartListDTO> data = mapper.Map<List<CartListDTO>>(list);
                return new ResponseDTO<List<CartListDTO>?>(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<CartListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<bool>> Create(CartCreateRemoveDTO DTO)
        {
            try
            {
                User? user = await daoUser.getUser(DTO.UserId);
                if (user == null)
                {
                    return new ResponseDTO<bool>(false, "Not found user", (int)HttpStatusCode.NotFound);
                }
                Product? product = await daoProduct.getProduct(DTO.ProductId);
                if (product == null)
                {
                    return new ResponseDTO<bool>(false, "Not found product", (int)HttpStatusCode.NotFound);
                }
                Cart? cart = await daoCart.getCart(DTO);
                if(cart == null)
                {
                    cart = new Cart()
                    {
                        CartId = Guid.NewGuid(),
                        UserId = DTO.UserId,
                        ProductId = DTO.ProductId,
                        Quantity = 1,
                        IsCheckout = false,
                        CreatedAt = DateTime.Now,
                        UpdateAt = DateTime.Now,
                        IsDeleted = false,
                    };
                    await daoCart.CreateCart(cart);
                }
                else
                {
                    cart.Quantity++;
                    cart.UpdateAt = DateTime.Now;
                    await daoCart.UpdateCart(cart);
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
