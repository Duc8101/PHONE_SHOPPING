using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.DTO.UserDTO;
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

        public async Task<ResponseDTO> Index(string? status, int? page)
        {
            try
            {
                int pageSelected = page == null ? 1 : page.Value;
                string URL = "https://localhost:7033/Order/List";
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
                ResponseDTO? result = Deserialize(data);
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
                return new ResponseDTO(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> Detail(Guid OrderID)
        {
            try
            {
                string URL = "https://localhost:7033/Order/Detail/" + OrderID;
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

        public async Task<ResponseDTO> Update(Guid OrderID, OrderUpdateDTO DTO)
        {
            try
            {
                string URL = "https://localhost:7033/Order/Update/" + OrderID;
                string requestData = JsonSerializer.Serialize(DTO);
                StringContent content = new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE); ;
                HttpResponseMessage response = await client.PutAsync(URL, content);
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
