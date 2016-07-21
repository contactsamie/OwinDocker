using Topshelf;

/*
    Install-Package Microsoft.AspNet.WebApi.Owin
    Install-Package Microsoft.Owin.Cors
    Install-Package Microsoft.Owin.SelfHost
    Install-Package nlog
    Install-Package NLog.Owin.Logging 
    Install-Package topshelf
    Install-Package topshelf.nlog
*/


namespace OwinDocker
{
    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(x =>                                 //1
            {
                x.Service<MyServiceClass>(s =>                        //2
                {
                    s.ConstructUsing(name => new MyServiceClass());     //3
                    s.WhenStarted(tc => tc.Start());              //4
                    s.WhenStopped(tc => tc.Stop());               //5
                });
                x.RunAsLocalSystem();                            //6

                x.SetDescription("Sample Topshelf Host");        //7
                x.SetDisplayName("Stuff");                       //8
                x.SetServiceName("Stuff");                       //9
            });                                                  //10
        }
    }
}