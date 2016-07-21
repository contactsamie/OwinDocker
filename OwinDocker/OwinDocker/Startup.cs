using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Owin.Logging;
using Owin;

namespace OwinDocker
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host.
            HttpConfiguration config = new HttpConfiguration();

            // Add Console Logger
            appBuilder.UseNLog();

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            config.Formatters.JsonFormatter.SerializerSettings =
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

            config.Routes.MapHttpRoute(
                name: "api",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            appBuilder.UseWebApi(config);
        }
    }
}