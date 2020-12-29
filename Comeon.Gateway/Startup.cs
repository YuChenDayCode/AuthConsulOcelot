using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace Comeon.Gateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            string AuthProviderKey = "GatewayKeyAuth"; //对应配置文件
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(AuthProviderKey, options =>
                {
                    options.Authority = "http://localhost:7200"; //id4 服务地址
                    options.ApiName = "UserApi";
                    options.RequireHttpsMetadata = false;        //不需要https
                    options.SupportedTokens = IdentityServer4.AccessTokenValidation.SupportedTokens.Both;
                });

            services.AddOcelot()                            //重点在配置文件 configuration.json
                .AddConsul()                                //使用consul治理服务 Ocelot.Provider.Consul
                .AddCacheManager(m =>                       //使用缓存 Ocelot.Cache.CacheManager  可实现IOcelotCache 自定义缓存
                {
                    m.WithDictionaryHandle();//默认字典存储
                })
                .AddPolly();                                //瞬态故障库 限流  Ocelot.Provider.Polly
        }

        // 配置请求管道
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseOcelot().Wait();

        }
    }
}