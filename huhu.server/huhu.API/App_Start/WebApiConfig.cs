using huhu.API.Filters;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiThrottle;

namespace huhu.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // 跨域配置
            var allowedMethods = ConfigurationManager.AppSettings["cors:allowedMethods"];
            var allowedOrigin = ConfigurationManager.AppSettings["cors:allowedOrigin"];
            var allowedHeaders = ConfigurationManager.AppSettings["cors:allowedHeaders"];
            var geduCors = new EnableCorsAttribute(allowedOrigin, allowedHeaders, allowedMethods)
            {
                SupportsCredentials = true
            };
            config.EnableCors(geduCors);
            // IP限流
            config.Filters.Add(new ThrottlingFilter()
            {
                QuotaExceededMessage = "操作频繁，请稍后重试！",
                QuotaExceededResponseCode = HttpStatusCode.ServiceUnavailable,
                Policy = new ThrottlePolicy(perSecond: 1, perMinute: 20,
                perHour: 200, perDay: 2000, perWeek: 10000)
                {
                    //ip配置区域
                    IpThrottling = true,
                    IpRules = new Dictionary<string, RateLimits>
                    {
                         { "::1/10", new RateLimits { PerSecond = 2 } },
                         { "192.168.2.1", new RateLimits { PerMinute = 30, PerHour = 30*60, PerDay = 30*60*24 } }
                    },
                    //添加127.0.0.1到白名单，本地地址不启用限流策略
                    IpWhitelist = new List<string> { "127.0.0.1", "192.168.0.0/24" },
                    //客户端配置区域，如果ip限制也是启动的，那么客户端限制策略会与ip限制策略组合使用。
                    ClientRules = new Dictionary<string, RateLimits>
                    {
                         { "api-client-key-demo", new RateLimits { PerDay = 5000 } }
                    },
                    //白名单中的客户端key不会进行限流。
                    ClientWhitelist = new List<string> { "admin-key" },
                    //端点限制策略配置会从EnableThrottling特性中获取。
                    EndpointThrottling = true
                }
            });
            // 路由
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // 日志
            config.Filters.Add(new ErrorLogFilter());
        }
    }
}
