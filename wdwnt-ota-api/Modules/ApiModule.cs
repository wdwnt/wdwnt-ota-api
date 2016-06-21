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
                var nowEst = NowEst();
                var suggestRadio = nowEst.DayOfWeek == DayOfWeek.Wednesday && (nowEst.Hour == 20 || nowEst.Hour == 21);

                dynamic response = new ExpandoObject();
                response.Ota_stream_url = "http://u.wdwnt.com/ListenOtA";
                response.Wbzw_stream_url = "http://14033.live.streamtheworld.com:3690/WBZWAMAAC_SC";
                response.Suggest_radio = suggestRadio;

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
                    response.Centova = new { Error = e.Message };
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