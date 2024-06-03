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

        public async Task<ResponseDTO> Index(string? status, int? page)
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
                ResponseDTO? result = Deserialize<ResponseDTO>(data);
                if (result == null)
                {
                    return new ResponseDTO(null, data, (int)response.StatusCode);
                }
                if (result.Data == null)
                {
                    return new ResponseDTO(null, result.Message, (int)response.StatusCode);
                }
                List<string> list = new List<string>();
                list.Add(OrderConst.STATUS_PENDING);
                list.Add(OrderConst.STATUS_APPROVED);
                list.Add(OrderConst.STATUS_REJECTED);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["result"] = result.Data;
                dic["list"] = list;
                dic["status"] = status == null ? "" : status.Trim();
                return new ResponseDTO(dic, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> View(Guid UserID)
        {
            try
            {
                string URL = "https://localhost:7033/User/Detail/" + UserID;
                HttpResponseMessage response = await GetAsync(URL);
                string data = await getResponseData(response);
                ResponseDTO? result = Deserialize<ResponseDTO>(data);
                if (result == null)
                {
                    return new ResponseDTO(null, data, (int)response.StatusCode);
                }
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseDTO(result.Data, string.Empty);
                }
                return new ResponseDTO(null, result.Message, (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> Detail(Guid OrderID)
        {
            try
            {
                string URL = "https://localhost:7033/Order/Detail/" + OrderID;
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

        public async Task<ResponseDTO> Update(Guid OrderID, OrderUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Order/Update/" + OrderID;
                string requestData = getRequestData<OrderUpdateDTO?>(DTO);
                StringContent content = getContent(requestData);
                HttpResponseMessage response = await PutAsync(URL, content);
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
