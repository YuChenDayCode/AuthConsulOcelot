using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Comeon.Web.Unit
{
    public class HttpClientHelp
    {
        private readonly IHttpClientFactory _iHttpClientFactory;
        public HttpClientHelp(IHttpClientFactory ihttpClientFactory)
        {
            _iHttpClientFactory = ihttpClientFactory;
        }

        /// <summary>
        /// Json对象转
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public string ConvertFormUrlencoded(object Obj)
        {
            Dictionary<string, object> children = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(Obj));
            return string.Join('&', children.Select(s => $"{s.Key}={s.Value}"));

        }
        public string ConvertFormUrlencoded(string Jsonstr)
        {
            Dictionary<string, object> children = JsonConvert.DeserializeObject<Dictionary<string, object>>(Jsonstr);
            return string.Join('&', children.Select(s => $"{s.Key}={s.Value}"));
        }
      
        /// <summary>
        /// multipart/form-data
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> Post(MultipartFormDataContent formData, string url)
        {
            var httpclient = _iHttpClientFactory.CreateClient();
            //HttpContent httpContent = new MultipartFormDataContent(re);
            //formData.Headers.ContentType = new MediaTypeHeaderValue("form-data");
            formData.Headers.ContentLength = formData.Count();
            Task<HttpResponseMessage> response = httpclient.PostAsync(url, formData);
            var stringresult = response.Result.Content.ReadAsStringAsync().Result;
            return stringresult;
        }


    }
}
