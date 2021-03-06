﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Guap.Service
{
    using System;

    using Guap.Contracts;

    using Plugin.Connectivity;

    using Xamarin.Forms;

    public class RequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;
        private IMessage _message;

        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            
            _serializerSettings.Converters.Add(new StringEnumConverter());
            _message = DependencyService.Get<IMessage>();
        }
        
        public async Task<TResult> GetAsync<TResult>(string uri)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                _message.ShortAlert("No internet connection! Cannot load data from server.");
                return default(TResult);
            }

            var httpClient = CreateHttpClient();
            HttpResponseMessage response;
            try
            {
                response = await httpClient.GetAsync(uri);

                await HandleResponse(response);
            }
            catch (HttpRequestException e)
            {
                _message.ShortAlert("Server error. Try again later.");
                return default(TResult);
            }
            catch (Exception e)
            {
                _message.ShortAlert("Something wrong. Try reload application.");
                return default(TResult);
            }
           
            return await DeserializeObject<TResult>(response);
        }
        
        public async Task<TResult> PostAsync<TData, TResult>(string uri, TData data)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                _message.ShortAlert("No internet connection! Cannot load data from server.");
                return default(TResult);
            }

            var httpClient = CreateHttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(data))
            {
                Headers = {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            };

            HttpResponseMessage response;

            try
            {
                response = await httpClient.PostAsync(uri, content);

                await HandleResponse(response);
            }
            catch (HttpRequestException e)
            {
                _message.ShortAlert("Server error. Try again later.");
                return default(TResult);
            }
            catch (Exception e)
            {
                _message.ShortAlert("Something wrong. Try reload application.");
                return default(TResult);
            }

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