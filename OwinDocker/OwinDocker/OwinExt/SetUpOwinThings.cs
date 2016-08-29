using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Security.ActiveDirectory;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Owin.Logging;
using Owin;

namespace OwinDocker.OwinExt
{
    public class SetUpOwinThings
    {
        public static void SetUpAuthentication(IAppBuilder app)
        {
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Audience = "https://contoso7.onmicrosoft.com/RichAPI",
                    Tenant = "contoso7.onmicrosoft.com"
                });
        }

        public static void SetUpIntegratedWindowsAuthentication(IAppBuilder app)
        {
            var listener =
                (HttpListener)app.Properties["System.Net.HttpListener"];
            listener.AuthenticationSchemes =
                AuthenticationSchemes.IntegratedWindowsAuthentication;
        }

        public static void SetUpFileServer(IAppBuilder app, string path)
        {
            var fileSystem = new PhysicalFileSystem(path);
            var options = new FileServerOptions
            {
                EnableDirectoryBrowsing = true,
                FileSystem = fileSystem,
                EnableDefaultFiles = true
            };

            app.UseFileServer(options);
        }

        public static void SetUpWebApi(IAppBuilder app, HttpConfiguration config)
        {
            // Configure Web API for self-host.
            //   HttpConfiguration config = new HttpConfiguration();

            // Add Console Logger
            app.UseNLog();

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

            app.UseWebApi(config);
        }
    }
}
