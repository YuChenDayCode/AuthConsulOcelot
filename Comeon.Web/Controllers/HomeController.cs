using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Mvc;

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

            string url = ConsulServices.GetServicesUrl();
            ViewBag.Info = url;

            var httpclient = _iHttpClientFactory.CreateClient();
            ViewBag.Result = await httpclient.GetStringAsync(url);

            return View();
        }

        #endregion
    }
}

