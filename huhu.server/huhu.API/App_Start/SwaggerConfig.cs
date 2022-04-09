using System.Web.Http;
using WebActivatorEx;
using huhu.API;
using Swashbuckle.Application;
using System;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace huhu.API
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    //swaggerÅäÖÃ
                    c.IncludeXmlComments(GetXmlCommentsPath());
                    c.SingleApiVersion("v1", "huhu");
                })
                .EnableSwaggerUi(c =>
                {
                    c.DocumentTitle("huhu");
                });
        }
        private static string GetXmlCommentsPath()
        {
            return String.Format(@"{0}\bin\huhu.API.xml", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}