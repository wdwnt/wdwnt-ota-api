using Newtonsoft.Json.Linq;
using System.Net;
using System.Web;
using WdwntOtaApi.Models;

namespace wdwnt_ota_api.BusinessLogic
{
    public class FastPassDataRetriever : IFastPassDataRetriever
    {
        public FastPass GetFastPassData()
        {
            var webClient = new WebClient();

            var airtimeObject = JObject.Parse(webClient.DownloadString("https://docker01.wdwnt.com/radio"));

            var track = airtimeObject["current"]["metadata"];

            return new FastPass
            {
                Album = HttpUtility.UrlDecode(((string)track["album_title"])),
                Artist = HttpUtility.UrlDecode((string)track["artist_name"]),
                Title = HttpUtility.UrlDecode((string)track["track_title"])
            };
        }
    }
}