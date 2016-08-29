using System;
using Microsoft.Owin.Hosting;

namespace OwinDocker
{
    public class MyServiceClass
    {
        public void Start()
        {
            var address = "http://*:9200/";
            // Start OWIN host
            using (WebApp.Start<Startup>(url: address))
            {
                Console.ReadKey();
            }
        }

        public void Stop()
        {

        }
    }
}