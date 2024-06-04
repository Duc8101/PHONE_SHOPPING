using DataAccess.Base;
using DataAccess.Const;
using MVC.Token;
using System.Net.Http.Headers;
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
            if (StaticToken.Token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", StaticToken.Token);
            }
        }

        internal ResponseBase? Deserialize(string responseData)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Deserialize<ResponseBase>(responseData, options);
        }
    }
}
