using Common.Base;
using Common.Const;
using Common.DTO.OrderDetailDTO;
using Common.DTO.OrderDTO;
using Common.DTO.UserDTO;
using Common.Pagination;
using MVC.Services.Base;

namespace MVC.Services.ManagerOrder
{
    public class ManagerOrderService : BaseService, IManagerOrderService
    {
        public ManagerOrderService(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseBase<Dictionary<string, object>?>> Index(string? status, int? page)
        {
            int pageSelected = page == null ? 1 : page.Value;
            string URL = "https://localhost:7178/Order/List";
            ResponseBase<Pagination<OrderListDTO>?> response;
            if (status == null || status.Trim().Length == 0)
            {
                response = await Get<Pagination<OrderListDTO>?>(URL, new KeyValuePair<string, object>("page", pageSelected));
            }
            else
            {
                response = await Get<Pagination<OrderListDTO>?>(URL, new KeyValuePair<string, object>("status", status.Trim())
                    , new KeyValuePair<string, object>("page", pageSelected));
            }
            if (response.Data == null)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, response.Message, response.Code);
            }
            List<string> list = new List<string>();
            list.Add(OrderConst.STATUS_PENDING);
            list.Add(OrderConst.STATUS_APPROVED);
            list.Add(OrderConst.STATUS_REJECTED);
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["result"] = response.Data;
            result["list"] = list;
            result["status"] = status == null ? "" : status.Trim();
            return new ResponseBase<Dictionary<string, object>?>(result, string.Empty);
        }

        public async Task<ResponseBase<UserDetailDTO?>> View(Guid UserID)
        {
            string URL = "https://localhost:7178/User/Detail/" + UserID;
            return await Get<UserDetailDTO?>(URL);
        }

        public async Task<ResponseBase<OrderDetailDTO?>> Detail(Guid OrderID)
        {
            string URL = "https://localhost:7178/Order/Detail/" + OrderID;
            return await Get<OrderDetailDTO?>(URL);
        }

        public async Task<ResponseBase<OrderDetailDTO?>> Update(Guid OrderID, OrderUpdateDTO DTO)
        {
            string URL = "https://localhost:7178/Order/Update/" + OrderID;
            return await Put<OrderUpdateDTO, OrderDetailDTO?>(URL, DTO);
        }
    }
}
