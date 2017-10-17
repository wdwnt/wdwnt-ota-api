using Newtonsoft.Json.Linq;
using System;
using System.Net;
using WdwntOtaApi.Models;

namespace wdwnt_ota_api.BusinessLogic
{
    public class CentovaDataRetriever : ICentovaDataRetriever
    {
        public Centova GetCentovaData()
        {
            var centovaInfo = new Centova();

            var webClient = new WebClient();

            var centovaObject =
                JObject.Parse(
                    webClient.DownloadString(
                        "http://panel2.directhostingcenter.com:2199/rpc/wukcrjdg/streaminfo.get"));

            var track = centovaObject["data"][0]["track"];

            if (!string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["TitleOverride"]))
            {
                var appSettings = System.Configuration.ConfigurationManager.AppSettings;
                centovaInfo.Album = appSettings["AlbumOverride"];
                centovaInfo.Artist = appSettings["ArtistOverride"];
                centovaInfo.Title = appSettings["TitleOverride"];
            }
            else if (((string)track["title"]).ToLower().Contains("streamtheworld"))
            {
                centovaInfo.Album = "Tom Corless";
                centovaInfo.Artist = "WDWNT";

                centovaInfo.Title = DateTimeCalculator.NowEst.DayOfWeek == DayOfWeek.Wednesday
                    ? "WDW News Tonight"
                    : "A Special Show!";
            }
            else
            {
                centovaInfo.Album = ((string)track["album"]).Replace(
                    "German Top 50 Official Dance Charts", "Disney");
                centovaInfo.Artist = (string)track["artist"];
                centovaInfo.Title = (string)track["title"];
            }

            return centovaInfo;
        }
    }
}