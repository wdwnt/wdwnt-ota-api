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
                var response = new Result();

                var nowEst = NowEst();
                response.Suggest_radio = nowEst.DayOfWeek == DayOfWeek.Wednesday &&
                                        (nowEst.Hour == 21 || nowEst.Hour == 22);

                try
                {
                    var webClient = new WebClient();
                    var centovaObject =
                        JObject.Parse(
                            webClient.DownloadString(
                                "http://panel2.directhostingcenter.com:2199/rpc/wukcrjdg/streaminfo.get"));

                    var track = centovaObject["data"][0]["track"];
                    if (((string) track["title"]).ToLower().Contains("streamtheworld"))
                    {
                        response.Centova.Album = "Tom Corless";
                        response.Centova.Artist = "WDWNT";

                        response.Centova.Title = nowEst.DayOfWeek == DayOfWeek.Wednesday
                            ? "WDW News Tonight"
                            : "A Special Show!";
                    }
                    else
                    {
                        response.Centova.Album = (string)track["album"];
                        response.Centova.Artist = (string)track["artist"];
                        response.Centova.Title = (string)track["title"];
                    }

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