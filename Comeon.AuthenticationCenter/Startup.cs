using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Comeon.AuthenticationCenter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();


            var res = new List<ApiResource> {    //资源访问限制
                    new ApiResource("UserApi","用户获取API")
                    {
                        Scopes = { "UserApi" }
                    }
                };
            var sco = new List<ApiScope> {    //
                    new ApiScope("UserApi")
                };
            IEnumerable<Client> client = new[]{   //InMemory 内存模式
                     new Client{
                          ClientId ="Comeon.Auth",
                          ClientSecrets = new []{ new Secret("a123456".Sha256())  },
                          AllowedGrantTypes = GrantTypes.ClientCredentials,
                          AllowedScopes ={ "UserApi"},
                          Claims = new List<ClientClaim>(){
                              //new ClientClaim(IdentityModel.JwtClaimTypes.Role,"Admin"),
                              new ClientClaim(IdentityModel.JwtClaimTypes.NickName,"testusr"),
                              //new ClientClaim("email","test@test.com")
                          }
                     }
                };
            services.AddIdentityServer()
                .AddDeveloperSigningCredential() //默认开发者临时证书
                .AddInMemoryClients(client) //添加验证信息
                .AddInMemoryApiScopes(sco) //api域限制
                .AddInMemoryApiResources(res); //resouces 资源保护

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer(); //添加id4   地址 connect/token

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
