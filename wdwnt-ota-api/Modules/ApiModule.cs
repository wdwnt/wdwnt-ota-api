using System.Net;
using Nancy;
using Newtonsoft.Json.Linq;

namespace wdwnt_ota_api.Modules
{
    public class ApiModule : NancyModule
    {
        public ApiModule() : base("/api/v1")
        {
            Get["/info"] = _ =>
            {
                var webClient = new WebClient();
                var centovaJson = webClient.DownloadString("http://panel2.directhostingcenter.com:2199/rpc/wukcrjdg/streaminfo.get");
                return Response.AsJson(new
                {
                    Centova = JObject.Parse(centovaJson)
                });
            };
        }
    }
}