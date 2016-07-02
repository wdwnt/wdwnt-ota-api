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
                response.Ota_stream_url = "http://audio.wdwntunes.com:8290/stream";
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
                                "http://panel2.directhostingcenter.com:2199/rpc/wukcrjdg/streaminfo.get"));

                    response.Centova = centovaObject;
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