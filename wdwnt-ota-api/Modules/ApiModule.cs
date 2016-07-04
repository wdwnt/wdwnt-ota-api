using System;
using System.Dynamic;
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
                dynamic response = new ExpandoObject();
                response.Ota_stream_url = "http://23.95.25.17:8142/";
                response.Wbzw_stream_url = "http://14033.live.streamtheworld.com:3690/WBZWAMAAC_SC";

                var nowEst = NowEst();
                response.Suggest_radio = nowEst.DayOfWeek == DayOfWeek.Wednesday &&
                                         (nowEst.Hour == 20 || nowEst.Hour == 21);

                try
                {
                    var webClient = new WebClient();
                    var centovaObject =
                        JObject.Parse(
                            webClient.DownloadString(
                                "http://23.95.25.17:2199/rpc/wdwntllc/streaminfo.get"));

                    response.Centova = centovaObject;
                    response.Error = null;
                }
                catch (Exception e)
                {
                    response.Centova = null;
                    response.Error = e.Message;
                }

                return Response.AsJson((object)response);
            };
        }

        public static DateTime NowEst()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }
    }
}