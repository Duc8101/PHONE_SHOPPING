using DataAccess.DTO;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.OrderDTO;
using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class CartService : BaseService, ICartService
    {
        public CartService() : base()
        {
        }

        public async Task<ResponseDTO> Create(CartCreateRemoveDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Cart/Create";
                string requestData = getRequestData<CartCreateRemoveDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO? result = Deserialize<ResponseDTO>(responseData);
                if (result == null)
                {
                    return new ResponseDTO(false, responseData, (int)response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO(true, string.Empty);
                }
                return new ResponseDTO(false, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> Index(string UserID)
        {
            try
            {
                string URL = "https://localhost:7033/Cart/List?UserID=" + UserID;
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO? result = Deserialize<ResponseDTO>(data);
                if (result == null)
                {
                    return new ResponseDTO(null, data, (int)response.StatusCode);
                }
                if (result.Data == null)
                {
                    return new ResponseDTO(null, result.Message, (int)response.StatusCode);
                }
                return new ResponseDTO(result.Data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> Remove(CartCreateRemoveDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Cart/Remove";
                string requestData = getRequestData<CartCreateRemoveDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO? result = Deserialize<ResponseDTO>(responseData);
                if (result == null)
                {
                    return new ResponseDTO(false, responseData, (int)response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO(true, string.Empty);
                }
                return new ResponseDTO(false, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> Checkout(OrderCreateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Order/Create";
                string requestData = getRequestData<OrderCreateDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PostAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO? result = Deserialize<ResponseDTO>(responseData);
                if (result == null)
                {
                    return new ResponseDTO(null, responseData, (int)response.StatusCode);
                }
                return new ResponseDTO(result.Data, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
