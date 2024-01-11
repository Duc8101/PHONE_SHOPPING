using DataAccess.Const;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MVC.Services
{
    public class BaseService
    {
        private readonly HttpClient client = new HttpClient();
        public BaseService()
        {
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue(OtherConst.MEDIA_TYPE);
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        protected async Task<HttpResponseMessage> GetAsync(string URL)
        {
            return await client.GetAsync(URL);
        }

        protected async Task<string> getResponseData(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        protected T? Deserialize<T>(string responseData)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Deserialize<T>(responseData, options);
        }

        protected string getRequestData<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        protected StringContent getContent(string requestData)
        {
            return new StringContent(requestData, Encoding.UTF8, OtherConst.MEDIA_TYPE);
        }

        protected async Task<HttpResponseMessage> PostAsync(string URL, StringContent content)
        {
            return await client.PostAsync(URL, content);
        }

        protected async Task<HttpResponseMessage> PutAsync(string URL, StringContent content)
        {
            return await client.PutAsync(URL, content);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string URL)
        {
            return await client.DeleteAsync(URL);
        }
    }
}
