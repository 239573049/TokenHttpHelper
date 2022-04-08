using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static TokenHelper.DelegaCommon;

namespace TokenHelper
{
    public class HttpHelper
    {
        /// <summary>
        /// 请求体拦截
        /// </summary>
        internal ResponseMessageHandling? _responseMessage;
        /// <summary>
        /// 响应拦截器
        /// </summary>
        internal RequestMessageHandling? _requestMessage;
        internal HttpClient _http;
        /// <summary>
        /// 请求格式
        /// </summary>
        public string _mediaType { get; set; } ="application/json";
        /// <summary>
        /// 默认创建
        /// </summary>
        public HttpHelper()
        {
            _http = HttpClientFactory.Create();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="http"></param>
        public HttpHelper(HttpClient http)
        {
            _http = http;
        }

        /// <summary>
        /// 发起Get请求（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="_headers">响应请求体信息</param>
        /// <returns></returns>
        public async Task<T?> GetAsync<T>(string url, ResponseMessageHandling? _headers = null) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Version = _http.DefaultRequestVersion,
                VersionPolicy = _http.DefaultVersionPolicy
            };
            _requestMessage?.Invoke(request);
            var message = await _http.SendAsync(request);
            _headers?.Invoke(message);
            _responseMessage?.Invoke(message);
            return await message.Content.ReadFromJsonAsync<T?>();
        }
        /// <summary>
        /// 发起Get请求（异步）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="_headers">响应请求体信息</param>
        /// <returns></returns>
        public async Task<string> GetAsync(string url, ResponseMessageHandling? _headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Version = _http.DefaultRequestVersion,
                VersionPolicy = _http.DefaultVersionPolicy
            };
            _requestMessage?.Invoke(request);
            var message = await _http.SendAsync(request);
            _headers?.Invoke(message);
            _responseMessage?.Invoke(message);
            return await message.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// 发起Get请求（异步）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="_headers">响应请求体信息</param>
        /// <returns></returns>
        public async Task<Stream> GetStreamAsync(string url, ResponseMessageHandling? _headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Version = _http.DefaultRequestVersion,
                VersionPolicy = _http.DefaultVersionPolicy
            };
            _requestMessage?.Invoke(request);
            var message = await _http.SendAsync(request);
            _headers?.Invoke(message);
            _responseMessage?.Invoke(message);
            return await message.Content.ReadAsStreamAsync();
        }
        /// <summary>
        /// 发起post请求（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <param name="_headers">响应请求体信息</param>
        /// <returns></returns>
        public async Task<T?> PostAsync<T>(string url, string value, ResponseMessageHandling? _headers = null) where T : class
        {
            HttpRequestMessage request = new(HttpMethod.Post, url);
            request.Version = _http.DefaultRequestVersion;
            request.VersionPolicy = _http.DefaultVersionPolicy;
            request.Content = new StringContent(value, Encoding.UTF8, _mediaType);
            _requestMessage?.Invoke(request);
            var message = await _http.SendAsync(request);
            _headers?.Invoke(message);
            _responseMessage?.Invoke(message);
            return await message.Content.ReadFromJsonAsync<T>();
        }
        /// <summary>
        /// 发起post请求（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <param name="_headers"></param>
        /// <returns></returns>
        public async Task<T?> PostAsync<T>(string url, object value, ResponseMessageHandling? _headers = null) where T : class
        {
            return await PostAsync<T>(url, JsonConvert.SerializeObject(value), _headers);
        }

        /// <summary>
        /// 发起post请求（异步）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <param name="_headers">响应请求体信息</param>
        /// <returns></returns>
        public async Task<string> PostAsync(string url, string value, ResponseMessageHandling? _headers = null)
        {
            HttpRequestMessage request = new(HttpMethod.Post, url);
            request.Version = _http.DefaultRequestVersion;
            request.VersionPolicy = _http.DefaultVersionPolicy;
            request.Content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, _mediaType);
            _requestMessage?.Invoke(request);
            var message = await _http.SendAsync(request);
            _headers?.Invoke(message);
            _responseMessage?.Invoke(message);
            return await message.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// 发起post请求（异步）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <param name="_headers"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string url, object value, ResponseMessageHandling? _headers = null)
        {
            return await PostAsync(url, JsonConvert.SerializeObject(value), _headers);
        }
        /// <summary>
        /// 发起Delete请求（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="_headers">响应请求体信息</param>
        /// <returns></returns>
        public async Task<T?> DeleteAsync<T>(string url, ResponseMessageHandling? _headers = null)
        {
            HttpRequestMessage request = new(HttpMethod.Delete, url);
            request.Version = _http.DefaultRequestVersion;
            request.VersionPolicy = _http.DefaultVersionPolicy;
            _requestMessage?.Invoke(request);
            var message = await _http.SendAsync(request);
            _headers?.Invoke(message);
            _responseMessage?.Invoke(message);
            return await message.Content.ReadFromJsonAsync<T>();
        }
        /// <summary>
        /// 发起Delete请求（异步）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="_headers">响应请求体信息</param>
        /// <returns></returns>
        public async Task<string> DeleteAsync(string url, ResponseMessageHandling? _headers = null)
        {
            HttpRequestMessage request = new(HttpMethod.Delete, url);
            request.Version = _http.DefaultRequestVersion;
            request.VersionPolicy = _http.DefaultVersionPolicy;
            _requestMessage?.Invoke(request);
            var message = await _http.SendAsync(request);
            _headers?.Invoke(message);
            _responseMessage?.Invoke(message);
            return await message.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// 发起Put请求（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <param name="_headers">响应请求体信息</param>
        /// <returns></returns>
        public async Task<T?> PutAsync<T>(string url, object value, ResponseMessageHandling? _headers = null)
        {
            HttpRequestMessage request = new(HttpMethod.Put, url);
            request.Version = _http.DefaultRequestVersion;
            request.VersionPolicy = _http.DefaultVersionPolicy;
            request.Content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, _mediaType);
            _requestMessage?.Invoke(request);
            var message = await _http.SendAsync(request);
            _headers?.Invoke(message);
            _responseMessage?.Invoke(message);
            return await message.Content.ReadFromJsonAsync<T>();
        }
        /// <summary>
        /// 发起Put请求（异步）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <param name="_headers">响应请求体信息</param>
        /// <returns></returns>
        public async Task<string> PutAsync(string url, object value, ResponseMessageHandling? _headers = null)
        {
            HttpRequestMessage request = new(HttpMethod.Put, url);
            request.Version = _http.DefaultRequestVersion;
            request.VersionPolicy = _http.DefaultVersionPolicy;
            request.Content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, _mediaType);
            _requestMessage?.Invoke(request);
            var message = await _http.SendAsync(request);
            _headers?.Invoke(message);
            _responseMessage?.Invoke(message);
            return await message.Content.ReadAsStringAsync();
        }
    }
}
