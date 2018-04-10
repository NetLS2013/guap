using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Guap.Service
{
    public class RequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;
        
        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }
        
        public async Task<TResult> GetAsync<TResult>(string uri)
        {
            var httpClient = CreateHttpClient();
            var response = await httpClient.GetAsync(uri);
            
            await HandleResponse(response);
            
            return await DeserializeObject<TResult>(response);
        }
        
        public async Task<TResult> PostAsync<TData, TResult>(string uri, TData data)
        {
            var httpClient = CreateHttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(data))
            {
                Headers = {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            };
            
            var response = await httpClient.PostAsync(uri, content);
            
            await HandleResponse(response);
            
            return await DeserializeObject<TResult>(response);
        }

        private async Task<TResult> DeserializeObject<TResult>(HttpResponseMessage responseMessage)
        {
            var serialized = await responseMessage.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings);
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            return httpClient;
        }
        
        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                
                throw new HttpRequestException(content);
            }
        }    
    }
}