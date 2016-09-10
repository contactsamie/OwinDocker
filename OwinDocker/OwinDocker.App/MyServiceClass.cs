using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;

namespace OwinDocker.App
{
    public class MyServiceClass
    {
        public void Start()
        {
            var address = "http://*:9200/";
            // Start OWIN host
            using (WebApp.Start<Startup>(url: address))
            {
                DataFactory();
                Console.ReadKey();
            }
        }

        public static int Total = 0;
        private static void DataFactory()
        {
            var data = new List<dynamic>();

            foreach (var i in Enumerable.Range(0, Total))
            {
                data.Add(new { Name = "Otto Clay", Age = 25, Country = 1, Address = "Ap #897-1459 Quam Avenue", Married = false });
            }
            Total++;
            if (Total%100 == 0)
            {
                Total = 0;
            }
            Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith(x =>
                {
                    GlobalHost.ConnectionManager.GetHubContext<OwinDockerHub>().Clients.All.inventoryData(data);
                    DataFactory();
                });
        }

        public void Stop()
        {

        }
    }
}