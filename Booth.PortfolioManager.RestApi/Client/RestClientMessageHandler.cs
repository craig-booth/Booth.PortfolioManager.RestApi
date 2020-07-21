using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.IO;

using Booth.PortfolioManager.RestApi.Serialization;

namespace Booth.PortfolioManager.RestApi.Client
{
    public interface IRestClientMessageHandler
    {
        string BaseURL { get; }
        string JwtToken { get; set; }
        Guid Portfolio { get; set; }

        Task<T> GetAsync<T>(string url);
        Task PostAsync<D>(string url, D data);
        Task<T> PostAsync<T, D>(string url, D data);
    }

    public class RestClientMessageHandler : IRestClientMessageHandler
    {
        public string BaseURL { get; }
        private readonly IRestClientSerializer _Serializer;

        private string _JwtToken;
        public string JwtToken
        {
            get { return _JwtToken; }
            set
            {
                _JwtToken = value;
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
            }
        }
        public Guid Portfolio { get; set; }
        private HttpClient _HttpClient { get; }

        public RestClientMessageHandler(string baseURL, IRestClientSerializer serializer)
            : this(baseURL, new HttpClient(), serializer)
        {
            _JwtToken = null;
        }

        public RestClientMessageHandler(string baseURL, HttpClient httpClient, IRestClientSerializer serializer)
        {
            BaseURL = baseURL;

            _HttpClient = httpClient;

            _HttpClient.BaseAddress = new Uri(baseURL);
            _HttpClient.DefaultRequestHeaders.Accept.Clear();
            _HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _Serializer = serializer;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var httpResponse = await _HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            if (!httpResponse.IsSuccessStatusCode)
                throw new RestException(httpResponse.StatusCode, httpResponse.ReasonPhrase);

            if (!((httpResponse.Content is object) && (httpResponse.Content.Headers.ContentType.MediaType == "application/json")))
                throw new RestException(HttpStatusCode.UnsupportedMediaType, "Content Type is not application/json");

            var contentStream = await httpResponse.Content.ReadAsStreamAsync();

            using (var streamReader = new StreamReader(contentStream))
            {
                var result = _Serializer.Deserialize<T>(streamReader);

                return result;
            }
        }

        public async Task PostAsync<D>(string url, D data)
        {
            var contentStream = new MemoryStream();

            using (var streamWriter = new StreamWriter(contentStream))
            {
                _Serializer.Serialize<D>(streamWriter, data);
                streamWriter.Flush();

                var content = new StreamContent(contentStream);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var httpResponse = await _HttpClient.PostAsync(url, content);
                if (!httpResponse.IsSuccessStatusCode)
                    throw new RestException(httpResponse.StatusCode, httpResponse.ReasonPhrase);
            }
        }

        public async Task<T> PostAsync<T, D>(string url, D data)
        { 
            var contentStream = new MemoryStream();
            var content = new StreamContent(contentStream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (var streamWriter = new StreamWriter(contentStream))
            {
                _Serializer.Serialize<D>(streamWriter, data);
                streamWriter.Flush();
            }

            var httpResponse = await _HttpClient.PostAsync(url, content);
            if (!httpResponse.IsSuccessStatusCode)
                throw new RestException(httpResponse.StatusCode, httpResponse.ReasonPhrase);


            var responseStream = await httpResponse.Content.ReadAsStreamAsync();
            using (var streamReader = new StreamReader(responseStream))
            {
                var result = _Serializer.Deserialize<T>(streamReader);
                return result;
            }
        }
    }
}
