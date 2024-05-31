using API.Services.IService;
using AutoMapper;
using DataAccess.DTO;
using DataAccess.DTO.CartDTO;
using DataAccess.Entity;
using DataAccess.Model.IDAO;
using System.Net;

namespace API.Services.Service
{
    public class CartService : BaseService, ICartService
    {
        private readonly IDAOUser _daoUser;
        private readonly IDAOCart _daoCart;
        private readonly IDAOProduct _daoProduct;
        public CartService(IMapper mapper, IDAOUser daoUser, IDAOCart daoCart, IDAOProduct daoProduct) : base(mapper)
        {
            _daoUser = daoUser;
            _daoCart = daoCart;
            _daoProduct = daoProduct;
        }

        public async Task<ResponseDTO<List<CartListDTO>?>> List(Guid UserID)
        {
            try
            {
                User? user = await _daoUser.getUser(UserID);
                if (user == null)
                {
                    return new ResponseDTO<List<CartListDTO>?>(null, "Not found user", (int)HttpStatusCode.NotFound);
                }
                List<Cart> list = await _daoCart.getList(UserID);
                List<CartListDTO> data = _mapper.Map<List<CartListDTO>>(list);
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
                User? user = await _daoUser.getUser(DTO.UserId);
                if (user == null)
                {
                    return new ResponseDTO<bool>(false, "Not found user", (int)HttpStatusCode.NotFound);
                }
                Product? product = await _daoProduct.getProduct(DTO.ProductId);
                if (product == null)
                {
                    return new ResponseDTO<bool>(false, "Not found product", (int)HttpStatusCode.NotFound);
                }
                Cart? cart = await _daoCart.getCart(DTO);
                if (cart == null)
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
                    await _daoCart.CreateCart(cart);
                }
                else
                {
                    cart.Quantity++;
                    cart.UpdateAt = DateTime.Now;
                    await _daoCart.UpdateCart(cart);
                }
                return new ResponseDTO<bool>(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<bool>> Remove(CartCreateRemoveDTO DTO)
        {
            try
            {
                User? user = await _daoUser.getUser(DTO.UserId);
                if (user == null)
                {
                    return new ResponseDTO<bool>(false, "Not found user", (int)HttpStatusCode.NotFound);
                }
                Product? product = await _daoProduct.getProduct(DTO.ProductId);
                if (product == null)
                {
                    return new ResponseDTO<bool>(false, "Product not exist", (int)HttpStatusCode.Conflict);
                }
                Cart? cart = await _daoCart.getCart(DTO);
                if (cart == null)
                {
                    return new ResponseDTO<bool>(false, "Cart not exist", (int)HttpStatusCode.Conflict);
                }
                else
                {
                    if (cart.Quantity == 1)
                    {
                        await _daoCart.DeleteCart(cart);
                    }
                    else
                    {
                        cart.Quantity--;
                        cart.UpdateAt = DateTime.Now;
                        await _daoCart.UpdateCart(cart);
                    }
                }
                return new ResponseDTO<bool>(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<bool>> Delete(Guid UserID)
        {
            try
            {
                User? user = await _daoUser.getUser(UserID);
                if (user == null)
                {
                    return new ResponseDTO<bool>(false, "Not found user", (int)HttpStatusCode.NotFound);
                }
                List<Cart> list = await _daoCart.getList(UserID);
                foreach (Cart cart in list)
                {
                    await _daoCart.DeleteCart(cart);
                }
                return new ResponseDTO<bool>(false, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
