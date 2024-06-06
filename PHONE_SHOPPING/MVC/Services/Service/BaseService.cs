using DataAccess.Base;
using MVC.Token;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MVC.Services.Service
{
    public class BaseService
    {
        private readonly HttpClient _client;
        public BaseService(HttpClient client)
        {
            _client = client;
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            if (StaticToken.Token != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", StaticToken.Token);
            }
        }

        private ResponseBase<T>? Deserialize<T>(string data)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Deserialize<ResponseBase<T>>(data, options);
        }

        internal async Task<ResponseBase<T>> Get<T>(string url, params KeyValuePair<string, object>[] parameters)
        {
            try
            {
                string param = "";
                if (parameters.Length > 0)
                {
                    param = "?";
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (i == 0)
                        {
                            param = param + parameters[i].Key + "=" + parameters[i].Value;
                        }
                        else
                        {
                            param = param + "&" + parameters[i].Key + "=" + parameters[i].Value;
                        }
                    }
                }
                url = url + param;
                HttpResponseMessage response = await _client.GetAsync(url);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<T>? result = Deserialize<T>(data);
                if (result == null)
                {
                    return new ResponseBase<T>(response.ReasonPhrase == null ? "" : response.ReasonPhrase, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<T>(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        internal async Task<ResponseBase<T>> Delete<T>(string url, params KeyValuePair<string, object>[] parameters)
        {
            try
            {
                string param = "";
                if (parameters.Length > 0)
                {
                    param = "?";
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (i == 0)
                        {
                            param = param + parameters[i].Key + "=" + parameters[i].Value;
                        }
                        else
                        {
                            param = param + "&" + parameters[i].Key + "=" + parameters[i].Value;
                        }
                    }
                }
                url = url + param;
                HttpResponseMessage response = await _client.DeleteAsync(url);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<T>? result = Deserialize<T>(data);
                if (result == null)
                {
                    return new ResponseBase<T>(response.ReasonPhrase == null ? "" : response.ReasonPhrase, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<T>(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        internal async Task<ResponseBase<Tout>> Post<Tin, Tout>(string url, Tin obj)
        {
            try
            {
                string body = JsonSerializer.Serialize(obj);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(url, content);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<Tout>? result = Deserialize<Tout>(data);
                if (result == null)
                {
                    return new ResponseBase<Tout>(response.ReasonPhrase == null ? "" : response.ReasonPhrase, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<Tout>(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        internal async Task<ResponseBase<Tout>> Put<Tin, Tout>(string url, Tin obj)
        {
            try
            {
                string body = JsonSerializer.Serialize(obj);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync(url, content);
                string data = await response.Content.ReadAsStringAsync();
                ResponseBase<Tout>? result = Deserialize<Tout>(data);
                if (result == null)
                {
                    return new ResponseBase<Tout>(response.ReasonPhrase == null ? "" : response.ReasonPhrase, (int)response.StatusCode);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseBase<Tout>(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }


    }
}
