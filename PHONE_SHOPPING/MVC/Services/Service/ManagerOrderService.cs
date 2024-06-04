using DataAccess.Base;
using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.DTO.UserDTO;
using DataAccess.Pagination;
using MVC.Services.IService;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MVC.Services.Service
{
    public class ManagerOrderService : BaseService, IManagerOrderService
    {
        public ManagerOrderService() : base()
        {
        }

        public async Task<ResponseBase<Dictionary<string, object>?>> Index(string? status, int? page)
        {
            try
            {
                int pageSelected = page == null ? 1 : page.Value;
                string URL = "https://localhost:7178/Order/List";
                if (status == null || status.Trim().Length == 0)
                {
                    URL = URL + "?page=" + pageSelected;
                }
                else
                {
                    URL = URL + "?status=" + status.Trim() + "&page=" + pageSelected;
                }
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<Pagination<OrderListDTO>?>? result = Deserialize<Pagination<OrderListDTO>?>(data);
                if (result == null)
                {
                    return new ResponseBase<Dictionary<string, object>?>(null, "Can't get data", (int)response.StatusCode);
                }
                if (result.Data == null)
                {
                    return new ResponseBase<Dictionary<string, object>?>(null, result.Message, (int)response.StatusCode);
                }
                List<string> list = new List<string>();
                list.Add(OrderConst.STATUS_PENDING);
                list.Add(OrderConst.STATUS_APPROVED);
                list.Add(OrderConst.STATUS_REJECTED);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["result"] = result.Data;
                dic["list"] = list;
                dic["status"] = status == null ? "" : status.Trim();
                return new ResponseBase<Dictionary<string, object>?>(dic, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase<Dictionary<string, object>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase<UserDetailDTO?>> View(Guid UserID)
        {
            try
            {
                string URL = "https://localhost:7178/User/Detail/" + UserID;
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<UserDetailDTO?>? result = Deserialize<UserDetailDTO?>(data);
                if (result == null)
                {
                    return new ResponseBase<UserDetailDTO?>(null, data, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<UserDetailDTO?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase<OrderDetailDTO?>> Detail(Guid OrderID)
        {
            try
            {
                string URL = "https://localhost:7178/Order/Detail/" + OrderID;
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<OrderDetailDTO?>? result = Deserialize<OrderDetailDTO?>(data);
                if (result == null)
                {
                    return new ResponseBase<OrderDetailDTO?>(null, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<OrderDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase<OrderDetailDTO?>> Update(Guid OrderID, OrderUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7178/Order/Update/" + OrderID;
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE); ;
                HttpResponseMessage response = await client.PutAsync(URL, content);
                string responseData = await response.Content.ReadAsStringAsync();
                ResponseBase<OrderDetailDTO?>? result = Deserialize<OrderDetailDTO?>(responseData);
                if (result == null)
                {
                    return new ResponseBase<OrderDetailDTO?>(null, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<OrderDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
