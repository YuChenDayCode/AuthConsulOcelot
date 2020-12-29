using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Comeon.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _iHttpClientFactory;
        public HomeController(IHttpClientFactory ihttpClientFactory)
        {
            _iHttpClientFactory = ihttpClientFactory;
        }


        public async Task<IActionResult> Index()
        {
            #region 获取Consul地址
            // 使用网关后不需要 发现服务的事交给网关继承的consul执行
            //string url = ConsulServices.GetServicesUrl();
            //ViewBag.Info = url;
            //var httpclient = _iHttpClientFactory.CreateClient();
            //ViewBag.Result = await httpclient.GetStringAsync(url);
            #endregion
            var httpclient = _iHttpClientFactory.CreateClient();

            #region 获取token令牌
            string authurl = "http://localhost:7200/connect/token";
            var authdata = new
            {
                client_id = "Comeon.Auth",
                client_secret = "a123456",
                grant_type = "client_credentials",
                scope = "UserApi"
            };

            Dictionary<string, object> children = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(authdata));
            StringBuilder str = new StringBuilder();
            foreach (var item in children)
            {
                str.Append($"{item.Key}={item.Value}&");
            }
            string re = str.ToString().TrimEnd('&');
            HttpContent httpContent = new StringContent(re);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            httpContent.Headers.ContentLength = new ASCIIEncoding().GetBytes(re).Length;
            var result = await httpclient.PostAsync(authurl, httpContent);
            var stringresult = result.Content.ReadAsStringAsync().Result;
            JObject detail = JObject.Parse(stringresult);
            #endregion

            string url = "http://127.0.0.1:6299";//网关地址
            ViewBag.Info = url;
            httpclient = _iHttpClientFactory.CreateClient();
            httpclient.BaseAddress = new Uri(url);
            string requestUrl = "/T/WeatherForecast/GetWeather";
            string authstr = detail["access_token"].ToString();
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authstr);
            ViewBag.Result = await httpclient.GetStringAsync(requestUrl);

            return View();
        }


    }
}

