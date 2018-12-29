using System.Linq;
using System.Web.Http;
using WebActivatorEx;
using Swashbuckle.Application;
using Ast.Api;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]
namespace Ast.Api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Ast.Api");
                    //设置接口描述xml路径地址
                    c.IncludeXmlComments(string.Format("{0}/App_Start/Ast.Api.XML", System.AppDomain.CurrentDomain.BaseDirectory));
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                })
                .EnableSwaggerUi(c =>
                {
                });
        }
    }
}