using MVC.Services.IService;
using System.Net;

namespace MVC.Services.Service
{
    public class HomeService : BaseService, IHomeService
    {
        private async Task<ResponseBase> getListCategory()
        {
            try
            {
                string URL = "https://localhost:7178/Category/List/All";
                HttpResponseMessage response = await client.GetAsync(URL);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase? result = Deserialize(data);
                if (result == null)
                {
                    return new ResponseBase(null, "Can't get data", (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase> Index(string? name, int? CategoryID, int? page)
        {
            try
            {
                ResponseBase resCategory = await getListCategory();
                return resCategory;
            }
            catch (Exception ex)
            {
                return new ResponseBase(null, ex + " " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
