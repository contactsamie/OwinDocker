using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinDocker.App
{
    public class HelloController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            var response = Request.CreateResponse<string>(HttpStatusCode.OK, "Hello!");
            return response;
        }
    }
}