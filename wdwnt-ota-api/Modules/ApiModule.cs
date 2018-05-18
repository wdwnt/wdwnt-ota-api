using System;
using System.Configuration;
using Nancy;
using WdwntOtaApi.Models;
using wdwnt_ota_api.BusinessLogic;

namespace wdwnt_ota_api.Modules
{
    public class ApiModule : BaseApiModule
    {
        private readonly IAirtimeDataRetriever airtimeDataRetriever;

        public ApiModule(IAirtimeDataRetriever airtimeDataRetriever) : base("v1")
        {
            this.airtimeDataRetriever = airtimeDataRetriever ?? throw new ArgumentNullException(nameof(airtimeDataRetriever));

            Get["/info"] = _ =>
            {
                var otaStreamUrl = ConfigurationManager.AppSettings["AirtimeStreamUrl"];
                var response = new ApiV1Result
                {
                    Ota_stream_url = !Request.Headers.UserAgent.Contains("Android") ?
                                        $"{otaStreamUrl}.m3u" :
                                        otaStreamUrl
                };

                response.Suggest_radio = false;

                try
                {
                    // shoehorn Airtime into Centova response
                    var airtimeResponse = this.airtimeDataRetriever.GetAirtimeData();
                    response.Centova = new Centova
                    {
                        Album = airtimeResponse.Album,
                        Artist = airtimeResponse.Artist,
                        Title = airtimeResponse.Title
                    };
                }
                catch (Exception e)
                {
                    response.Centova = null;
                    response.Error = e.Message;
                    var errorResponse = Response.AsJson((object)response);
                    errorResponse.StatusCode = HttpStatusCode.InternalServerError;
                    return errorResponse;
                }

                return Response.AsJson((object)response);
            };
        }
    }
}
