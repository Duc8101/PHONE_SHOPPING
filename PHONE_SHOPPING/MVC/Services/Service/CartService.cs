using DataAccess.Const;
using DataAccess.DTO;
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

        public async Task<ResponseDTO> Create(CartCreateRemoveDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Cart/Create";
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PostAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseDTO? result = Deserialize(responseData);
                if (result == null)
                {
                    return new ResponseDTO(false, responseData, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> Index(string UserID)
        {
            try
            {
                string URL = "https://localhost:7033/Cart/List?UserID=" + UserID;
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseDTO? result = Deserialize(data);
                if (result == null)
                {
                    return new ResponseDTO(null, data, (int)response.StatusCode);
                }
                return result;
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
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PostAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseDTO? result = Deserialize(responseData);
                if (result == null)
                {
                    return new ResponseDTO(false, responseData, (int)response.StatusCode);
                }
                return result;
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
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
                HttpResponseMessage response = await client.PostAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseDTO? result = Deserialize(responseData);
                if (result == null)
                {
                    return new ResponseDTO(null, responseData, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
