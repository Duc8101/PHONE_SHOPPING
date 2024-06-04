using DataAccess.Base;
using DataAccess.DTO;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.Pagination;
using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class MyOrderService : BaseService, IMyOrderService
    {
        public MyOrderService() : base()
        {
        }

        public async Task<ResponseBase<Pagination<OrderListDTO>?>> Index(int? page)
        {
            try
            {
                int pageSelected = page == null ? 1 : page.Value;
                string URL = "https://localhost:7178/Order/List?page=" + pageSelected;
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<Pagination<OrderListDTO>?>? result = Deserialize<Pagination<OrderListDTO>?>(data);
                if (result == null)
                {
                    return new ResponseBase<Pagination<OrderListDTO>?>(null, data, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<Pagination<OrderListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase<OrderDetailDTO?>> Detail(Guid OrderID, Guid UserID)
        {
            try
            {
                string URL = "https://localhost:7178/Order/Detail/" + OrderID;
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<OrderDetailDTO?>? result = Deserialize<OrderDetailDTO?>(data);
                if (result == null)
                {
                    return new ResponseBase<OrderDetailDTO?>(null, data, (int)response.StatusCode);
                }
                if (result.Data == null)
                {
                    return new ResponseBase<OrderDetailDTO?>(null, result.Message, (int)response.StatusCode);
                }
                if (result.Data.UserId != UserID)
                {
                    return new ResponseBase<OrderDetailDTO?>(null, "Not found order match user", (int)HttpStatusCode.NotFound);
                }
                return new ResponseBase<OrderDetailDTO?>(result.Data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase<OrderDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
