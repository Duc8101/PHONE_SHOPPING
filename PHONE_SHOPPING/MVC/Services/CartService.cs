using DataAccess.DTO;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.OrderDTO;
using System.Net;

namespace MVC.Services
{
    public class CartService : BaseService
    {
        public CartService(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseDTO<bool>> Create(CartCreateRemoveDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Cart/Create";
                string requestData = getRequestData<CartCreateRemoveDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO<bool>? result = Deserialize<ResponseDTO<bool>>(responseData);
                if (result == null)
                {
                    return new ResponseDTO<bool>(false, responseData, (int)response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO<bool>(true, string.Empty);
                }
                return new ResponseDTO<bool>(false, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<List<CartListDTO>?>> Index(string UserID)
        {
            try
            {
                string URL = "https://localhost:7033/Cart/List?UserID=" + UserID;
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO<List<CartListDTO>?>? result = Deserialize<ResponseDTO<List<CartListDTO>?>>(data);
                if (result == null)
                {
                    return new ResponseDTO<List<CartListDTO>?>(null, data, (int)response.StatusCode);
                }
                if (result.Data == null)
                {
                    return new ResponseDTO<List<CartListDTO>?>(null, result.Message, (int)response.StatusCode);
                }
                return new ResponseDTO<List<CartListDTO>?>(result.Data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<CartListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<bool>> Remove(CartCreateRemoveDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Cart/Remove";
                string requestData = getRequestData<CartCreateRemoveDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO<bool>? result = Deserialize<ResponseDTO<bool>>(responseData);
                if (result == null)
                {
                    return new ResponseDTO<bool>(false, responseData, (int)response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO<bool>(true, string.Empty);
                }
                return new ResponseDTO<bool>(false, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<List<CartListDTO>?>> Checkout(OrderCreateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Order/Create";
                string requestData = getRequestData<OrderCreateDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO<List<CartListDTO>?>? result = Deserialize<ResponseDTO<List<CartListDTO>?>>(responseData);
                if (result == null)
                {
                    return new ResponseDTO<List<CartListDTO>?>(null, responseData, (int)response.StatusCode);
                }
                return new ResponseDTO<List<CartListDTO>?>(result.Data, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<CartListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
