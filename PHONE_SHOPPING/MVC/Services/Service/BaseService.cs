using DataAccess.Const;
using DataAccess.DTO;
using MVC.Token;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MVC.Services.Service
{
    public class BaseService
    {
        internal readonly HttpClient client = new HttpClient();
        public BaseService()
        {
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue(OtherConst.MEDIA_TYPE);
            client.DefaultRequestHeaders.Accept.Add(contentType);
            if(StaticToken.Token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", StaticToken.Token);
            }
           
        }

        internal ResponseDTO? Deserialize(string responseData)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Deserialize<ResponseDTO>(responseData, options);
        }
    }
}
