using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comeon.Web
{
    public class ConsulServices
    {
        private static long FLAG = 0;
        public static string GetServicesUrl()
        {

            ConsulClient consulClient = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500/");
                c.Datacenter = "dcl";
            });

            var response = consulClient.Agent.Services().Result.Response;

            string url = "http://ApiService/WeatherForecast/GetWeather";
            Uri u = new Uri(url);
            AgentService agentService = null;

            ///筛选当前服务的实例
            var serviceDictionary = response.Where(t => t.Value.Service.Equals(u.Host, StringComparison.OrdinalIgnoreCase)).ToArray();
            {
                long index = FLAG++ % serviceDictionary.Length;
                agentService = serviceDictionary[index].Value;
            };

            url = $"{u.Scheme}://{agentService.Address}:{agentService.Port}{u.PathAndQuery}";

            Console.WriteLine(url);
            return url;
        }
    }
}
