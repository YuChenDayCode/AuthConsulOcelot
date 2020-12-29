using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;

namespace Comeon.ServiceInstance
{
    public static class ConsulRegistServer
    {
        public static void ConsulRegist(this IConfiguration configuration)
        {
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500/");
                c.Datacenter = "dcl";
            });
            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);


            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = $"Service{ip}:{port}",
                Name = "TestService",
                Address = ip,
                Port = port
            });

        }
    }
}
