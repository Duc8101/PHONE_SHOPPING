using DataAccess.DTO;
using DataAccess.DTO.OrderDetailDTO;
using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class MyOrderService : BaseService, IMyOrderService
    {
        public MyOrderService() : base()
        {
        }

        public async Task<ResponseDTO> Index(string UserID, int? page)
        {
            try
            {
                int pageSelected = page == null ? 1 : page.Value;
                string URL = "https://localhost:7033/Order/List?UserID=" + UserID + "&isAdmin=false" + "&page=" + pageSelected;
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
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> Detail(Guid OrderID, Guid UserID)
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
                if (((OrderDetailDTO)result.Data).UserId != UserID)
                {
                    return new ResponseDTO(null, "Not found order", (int)HttpStatusCode.NotFound);
                }
                return new ResponseDTO(result.Data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
