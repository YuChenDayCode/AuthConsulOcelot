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
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //string AuthProviderKey = "UserGatewayKey";
            //services.AddAuthentication("Bearer")
            //    .AddIdentityServerAuthentication(AuthProviderKey, options =>
            //    {
            //        options.Authority = "http://localhost:7200"; //id4 �����ַ
            //        options.ApiName = "UserApi";
            //        options.RequireHttpsMetadata = false; //����Ҫhttps
            //        options.SupportedTokens = IdentityServer4.AccessTokenValidation.SupportedTokens.Both;
            //    });

            services.AddOcelot() //�ص��������ļ� configuration.json
                .AddConsul()    //ʹ��consul������� Ocelot.Provider.Consul
                .AddCacheManager(m =>   //ʹ�û��� Ocelot.Cache.CacheManager  ��ʵ��IOcelotCache �Զ��建��
                {
                    m.WithDictionaryHandle();//Ĭ���ֵ�洢
                })
                .AddPolly(); //˲̬���Ͽ� ����  Ocelot.Provider.Polly
        }

        // ��������ܵ�
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseOcelot().Wait();

        }
    }
}
