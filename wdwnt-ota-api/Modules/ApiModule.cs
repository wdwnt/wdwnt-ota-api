using System;
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
                var centovaJson = string.Empty;
                try
                {
                    var webClient = new WebClient();
                    centovaJson =
                        JObject.Parse(
                            webClient.DownloadString(
                                "http://panel2.directhostingcenter.com:2199/rpc/wukcrjdg/streaminfo.get")).ToString();
                }
                catch (Exception e)
                {
                    centovaJson = e.Message;
                }

                var nowEst = NowEst();
                var suggestRadio = nowEst.DayOfWeek == DayOfWeek.Wednesday && (nowEst.Hour == 20 || nowEst.Hour == 21);
                
                return Response.AsJson(new
                {
                    Centova = centovaJson,
                    Ota_stream_url = "http://u.wdwnt.com/ListenOtA",
                    Wbzw_stream_url = "http://14033.live.streamtheworld.com:3690/WBZWAMAAC_SC",
                    Suggest_radio = suggestRadio
                });
            };
        }

        public static DateTime NowEst()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }
    }
}