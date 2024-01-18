using DataAccess.DTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.DTO.UserDTO;
using System.Net;

namespace MVC.Services
{
    public class MyOrderService : BaseService
    {
        public async Task<ResponseDTO<PagedResultDTO<OrderListDTO>?>> Index(string UserID, int? page)
        {
            try
            {
                int pageSelected = page == null ? 1 : page.Value;
                string URL = "https://localhost:7033/Order/List?UserID=" + UserID + "&page=" + pageSelected;
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO<PagedResultDTO<OrderListDTO>?>? result = Deserialize<ResponseDTO<PagedResultDTO<OrderListDTO>?>>(data);
                if(result == null)
                {
                    return new ResponseDTO<PagedResultDTO<OrderListDTO>?>(null, data, (int)response.StatusCode);
                }
                if(result.Data == null)
                {
                    return new ResponseDTO<PagedResultDTO<OrderListDTO>?>(null, result.Message, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO<PagedResultDTO<OrderListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
