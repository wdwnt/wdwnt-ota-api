using Nancy.Bootstrapper;

namespace wdwnt_ota_api
{
    public class ApplicationStartup : IApplicationStartup
    {
        public void Initialize(IPipelines pipelines)
        {
            pipelines.AfterRequest += ctx =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, Content-Type, Accept");
            };
        }
    }
}