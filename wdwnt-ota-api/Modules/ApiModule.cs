using System;
using System.Net;
using Nancy;
using Newtonsoft.Json.Linq;
using wdwnt_ota_api.Models;

namespace wdwnt_ota_api.Modules
{
    public class ApiModule : NancyModule
    {
        public ApiModule() : base("/api/v1")
        {
            Get["/info"] = _ =>
            {
                var response = new Result
                {
                    OtaStreamUrl = "http://23.95.25.17:8142/",
                    WbzwStreamUrl = "http://14033.live.streamtheworld.com:3690/WBZWAMAAC_SC"
                };

                var nowEst = NowEst();
                response.SuggestRadio = nowEst.DayOfWeek == DayOfWeek.Wednesday &&
                                         (nowEst.Hour == 20 || nowEst.Hour == 21);

                try
                {
                    var webClient = new WebClient();
                    var centovaObject =
                        JObject.Parse(
                            webClient.DownloadString(
                                "http://23.95.25.17:2199/rpc/wdwntllc/streaminfo.get"));

                    var track = centovaObject["data"][0]["track"];
                    response.Centova.Album = (string) track["album"];
                    response.Centova.Artist = (string)track["artist"];
                    response.Centova.Title = (string)track["title"];

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