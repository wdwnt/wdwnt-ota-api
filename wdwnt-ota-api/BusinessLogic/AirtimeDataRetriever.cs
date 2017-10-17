using Newtonsoft.Json.Linq;
using System;
using System.Net;
using WdwntOtaApi.Models;

namespace wdwnt_ota_api.BusinessLogic
{
    public class AirtimeDataRetriever : IAirtimeDataRetriever
    {
        public Airtime GetAirtimeData()
        {
            var airtimeInfo = new Airtime();

            var webClient = new WebClient();

            var airtimeObject =
                JObject.Parse(
                    webClient.DownloadString(
                        "https://wdwnt.airtime.pro/api/live-info"));

            var track = airtimeObject["current"]["metadata"];

            if (!string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["TitleOverride"]))
            {
                var appSettings = System.Configuration.ConfigurationManager.AppSettings;
                airtimeInfo.Album = appSettings["AlbumOverride"];
                airtimeInfo.Artist = appSettings["ArtistOverride"];
                airtimeInfo.Title = appSettings["TitleOverride"];
            }
            else if (((string)track["track_title"]).ToLower().Contains("streamtheworld"))
            {
                airtimeInfo.Album = "Tom Corless";
                airtimeInfo.Artist = "WDWNT";

                airtimeInfo.Title = DateTimeCalculator.NowEst.DayOfWeek == DayOfWeek.Wednesday
                    ? "WDW News Tonight"
                    : "A Special Show!";
            }
            else
            {
                airtimeInfo.Album = ((string)track["album_title"]);
                airtimeInfo.Artist = (string)track["artist_name"];
                airtimeInfo.Title = (string)track["track_title"];
            }

            return airtimeInfo;
        }
    }
}