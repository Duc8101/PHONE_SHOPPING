using DataAccess.Base;
using DataAccess.Const;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.OrderDTO;
using MVC.Services.IService;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MVC.Services.Service
{
    public class CartService : BaseService, ICartService
    {
        public CartService() : base()
        {
        }

        public async Task<ResponseBase<bool>> Create(CartCreateRemoveDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7178/Cart/Create";
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PostAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseBase<bool>? result = Deserialize<bool>(responseData);
                if (result == null)
                {
                    return new ResponseBase<bool>(false, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase<List<CartListDTO>?>> Index()
        {
            try
            {
                string URL = "https://localhost:7178/Cart/List";
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<List<CartListDTO>?>? result = Deserialize<List<CartListDTO>?>(data);
                if (result == null)
                {
                    return new ResponseBase<List<CartListDTO>?>(null, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<CartListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase<bool>> Remove(CartCreateRemoveDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7178/Cart/Remove";
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PostAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseBase<bool>? result = Deserialize<bool>(responseData);
                if (result == null)
                {
                    return new ResponseBase<bool>(false, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase<List<CartListDTO>?>> Checkout(OrderCreateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7178/Order/Create";
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PostAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseBase<List<CartListDTO>?>? result = Deserialize<List<CartListDTO>?>(responseData);
                if (result == null)
                {
                    return new ResponseBase<List<CartListDTO>?>(null, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<CartListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
