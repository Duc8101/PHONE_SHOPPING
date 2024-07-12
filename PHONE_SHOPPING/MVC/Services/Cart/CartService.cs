using Common.Base;
using Common.DTO.CartDTO;
using Common.DTO.OrderDTO;
using MVC.Services.Base;

namespace MVC.Services.Cart
{
    public class CartService : BaseService, ICartService
    {

        public async Task<ResponseBase<bool?>> Create(CartCreateDTO DTO)
        {
            string URL = "https://localhost:7077/Cart/Create";
            return await Post<CartCreateDTO, bool?>(URL, DTO);
        }

        public async Task<ResponseBase<List<CartListDTO>?>> Index()
        {
            string URL = "https://localhost:7077/Cart/List";
            return await Get<List<CartListDTO>?>(URL);
        }

        public async Task<ResponseBase<bool?>> Remove(Guid productId)
        {
            string URL = "https://localhost:7077/Cart/Delete";
            return await Delete<bool?>(URL, new KeyValuePair<string, object>("productId", productId));
        }

        public async Task<ResponseBase<List<CartListDTO>?>> Checkout(OrderCreateDTO DTO)
        {
            string URL = "https://localhost:7077/Order/Create";
            return await Post<OrderCreateDTO, List<CartListDTO>?>(URL, DTO);
        }

    }
}
