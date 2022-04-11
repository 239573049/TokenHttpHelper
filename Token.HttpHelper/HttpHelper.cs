using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Token.HttpHelper;
using static TokenHelper.DelegaCommon;

namespace TokenHelper
{
    public class HttpHelper
    {
        /// <summary>
        /// 全局cookie
        /// </summary>
        public CookieContainer cookie = new CookieContainer();
        /// <summary>
        /// 请求体拦截
        /// </summary>
        public ResponseMessageHandling? _responseMessage;
        /// <summary>
        /// 响应拦截器
        /// </summary>
        public RequestMessageHandling? _requestMessage;

        public Dictionary<string,string> Headers { get; set; }=new Dictionary<string,string>();
        private HttpWebRequest CreateHttpWebRequest(string url,string? content =null, HttpMethodEnum method=HttpMethodEnum.GET)
        {
            var req = (HttpWebRequest) WebRequest.Create(url);
            req.Method = method.ToString();
            req.CookieContainer = cookie;
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.60 Safari/537.36 Edg/100.0.1185.29";
            req.Timeout = 60000;
            req.ContentType = "application/json;charset=UTF-8";
            if (Headers != null)
            {
                foreach (var d in Headers)
                {
                    req.Headers.Add(d.Key, d.Value);
                }
            }
            byte[] data = Encoding.UTF8.GetBytes(content);
            req.ContentLength = data.Length;
            if (method != HttpMethodEnum.GET)
            {
                using Stream reqStream = req.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            _responseMessage?.Invoke(req);
            return req;
        }

        private HttpWebRequest CreateHttpWebRequest(string url, object content, HttpMethodEnum method)=>
            CreateHttpWebRequest(url, JsonConvert.SerializeObject(content), method);

        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<T> GetResponseAsync<T>(HttpWebRequest request)
        {
            return await Task.Run(() =>
            {
                string result = string.Empty;
                HttpWebResponse resp = (HttpWebResponse) request.GetResponse();
                _requestMessage?.Invoke(resp);
                Stream stream = resp.GetResponseStream();

                //获取内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<T>(result)!;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string url)
        {
            return await GetResponseAsync<T>(CreateHttpWebRequest(url));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string url, string content)
        {
            return await GetResponseAsync<T>(CreateHttpWebRequest(url, content, HttpMethodEnum.POST));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string url, object content)
        {
            return await GetResponseAsync<T>(CreateHttpWebRequest(url, content, HttpMethodEnum.POST));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<T> PutAsync<T>(string url, string content)
        {
            return await GetResponseAsync<T>(CreateHttpWebRequest(url, content, HttpMethodEnum.PUT));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<T> PutAsync<T>(string url, object content)
        {
            return await GetResponseAsync<T>(CreateHttpWebRequest(url, content, HttpMethodEnum.PUT));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<T> DeleteAsync<T>(string url, string content)
        {
            return await GetResponseAsync<T>(CreateHttpWebRequest(url, content, HttpMethodEnum.DELETE));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<T> DeleteAsync<T>(string url, object content)
        {
            return await GetResponseAsync<T>(CreateHttpWebRequest(url, content, HttpMethodEnum.DELETE));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<T> MethodAsync<T>(string url, object content, HttpMethodEnum httpMethod)
        {
            return await GetResponseAsync<T>(CreateHttpWebRequest(url, content, httpMethod));
        }

    }
}
