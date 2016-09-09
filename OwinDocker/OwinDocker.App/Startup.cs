using System;
using System.Web.Http;
using Owin;

namespace OwinDocker.App
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