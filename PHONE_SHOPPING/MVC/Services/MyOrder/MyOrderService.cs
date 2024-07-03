using Common.Base;
using Common.DTO.OrderDetailDTO;
using Common.DTO.OrderDTO;
using Common.Pagination;
using MVC.Services.Base;
using System.Net;

namespace MVC.Services.MyOrder
{
    public class MyOrderService : BaseService, IMyOrderService
    {
        public MyOrderService(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseBase<Pagination<OrderListDTO>?>> Index(int? page)
        {
            int pageSelected = page == null ? 1 : page.Value;
            string URL = "https://localhost:7077/Order/List";
            return await Get<Pagination<OrderListDTO>?>(URL, new KeyValuePair<string, object>("page", pageSelected));
        }

        public async Task<ResponseBase<OrderDetailDTO?>> Detail(Guid OrderID, Guid UserID)
        {
            string URL = "https://localhost:7077/Order/Detail/" + OrderID;
            ResponseBase<OrderDetailDTO?> response = await Get<OrderDetailDTO?>(URL);
            if (response.Data == null)
            {
                return new ResponseBase<OrderDetailDTO?>(null, response.Message, response.Code);
            }
            if (response.Data.UserId != UserID)
            {
                return new ResponseBase<OrderDetailDTO?>(null, "Not found order match user", (int)HttpStatusCode.NotFound);
            }
            return new ResponseBase<OrderDetailDTO?>(response.Data, string.Empty);
        }
    }
}
