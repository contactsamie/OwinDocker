using System;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Owin.Logging;
using Owin;
using OwinDocker.OwinExt;

namespace OwinDocker
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            SetUpOwinThings.SetUpWebApi(appBuilder, config);
            SetUpOwinThings.SetUpFileServer(appBuilder, AppDomain.CurrentDomain.BaseDirectory + "web");
            SetUpOwinThings.SetUpIntegratedWindowsAuthentication(appBuilder);
        }
    }
}