using DataAccess.Const;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MVC.Services.Service
{
    public class BaseService
    {
        private readonly HttpClient client = new HttpClient();
        public BaseService()
        {
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue(OtherConst.MEDIA_TYPE);
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        internal async Task<HttpResponseMessage> GetAsync(string URL)
        {
            return await client.GetAsync(URL);
        }

        internal async Task<string> getResponseData(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        internal T? Deserialize<T>(string responseData)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Deserialize<T>(responseData, options);
        }

        internal string getRequestData<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        internal StringContent getContent(string requestData)
        {
            return new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
        }

        internal async Task<HttpResponseMessage> PostAsync(string URL, StringContent content)
        {
            return await client.PostAsync(URL, content);
        }

        internal async Task<HttpResponseMessage> PutAsync(string URL, StringContent content)
        {
            return await client.PutAsync(URL, content);
        }

        internal async Task<HttpResponseMessage> DeleteAsync(string URL)
        {
            return await client.DeleteAsync(URL);
        }
    }
}
