using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.DTO.UserDTO;
using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class ManagerOrderService : BaseService, IManagerOrderService
    {
        public ManagerOrderService() : base()
        {
        }

        public async Task<ResponseDTO<Dictionary<string, object>?>> Index(string? status, int? page)
        {
            try
            {
                int pageSelected = page == null ? 1 : page.Value;
                string URL = "https://localhost:7033/Order/List";
                if (status == null || status.Trim().Length == 0)
                {
                    URL = URL + "?isAdmin=true&page=" + pageSelected;
                }
                else
                {
                    URL = URL + "?status=" + status.Trim() + "&isAdmin=true&page=" + pageSelected;
                }
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO<PagedResultDTO<OrderListDTO>?>? result = Deserialize<ResponseDTO<PagedResultDTO<OrderListDTO>?>>(data);
                if (result == null)
                {
                    return new ResponseDTO<Dictionary<string, object>?>(null, data, (int)response.StatusCode);
                }
                if (result.Data == null)
                {
                    return new ResponseDTO<Dictionary<string, object>?>(null, result.Message, (int)response.StatusCode);
                }
                List<string> list = new List<string>();
                list.Add(OrderConst.STATUS_PENDING);
                list.Add(OrderConst.STATUS_APPROVED);
                list.Add(OrderConst.STATUS_REJECTED);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["result"] = result.Data;
                dic["list"] = list;
                dic["status"] = status == null ? "" : status.Trim();
                return new ResponseDTO<Dictionary<string, object>?>(dic, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<Dictionary<string, object>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<UserDetailDTO?>> View(Guid UserID)
        {
            try
            {
                string URL = "https://localhost:7033/User/Detail/" + UserID;
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO<UserDetailDTO?>? result = Deserialize<ResponseDTO<UserDetailDTO?>>(data);
                if (result == null)
                {
                    return new ResponseDTO<UserDetailDTO?>(null, data, (int)response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO<UserDetailDTO?>(result.Data, string.Empty);
                }
                return new ResponseDTO<UserDetailDTO?>(null, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDetailDTO?>(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<OrderDetailDTO?>> Detail(Guid OrderID)
        {
            try
            {
                string URL = "https://localhost:7033/Order/Detail/" + OrderID;
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO<OrderDetailDTO?>? result = Deserialize<ResponseDTO<OrderDetailDTO?>>(data);
                if (result == null)
                {
                    return new ResponseDTO<OrderDetailDTO?>(null, data, (int)response.StatusCode);
                }
                if (result.Data == null)
                {
                    return new ResponseDTO<OrderDetailDTO?>(null, result.Message, (int)response.StatusCode);
                }
                return new ResponseDTO<OrderDetailDTO?>(result.Data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<OrderDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<OrderDetailDTO?>> Update(Guid OrderID, OrderUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Order/Update/" + OrderID;
                string requestData = getRequestData<OrderUpdateDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PutAsync(URL, content);
                string responseData = await getResponseData(response);
                ResponseDTO<OrderDetailDTO?>? result = Deserialize<ResponseDTO<OrderDetailDTO?>>(responseData);
                if (result == null)
                {
                    return new ResponseDTO<OrderDetailDTO?>(null, responseData, (int)response.StatusCode);
                }
                return new ResponseDTO<OrderDetailDTO?>(result.Data, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<OrderDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
