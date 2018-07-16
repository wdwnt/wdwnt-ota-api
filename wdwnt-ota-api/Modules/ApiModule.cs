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
        private readonly IFastPassDataRetriever fastPassDataRetriever;

        public ApiModule(IAirtimeDataRetriever airtimeDataRetriever, IFastPassDataRetriever fastPassDataRetriever) : base("v1")
        {
            this.airtimeDataRetriever = airtimeDataRetriever ?? throw new ArgumentNullException(nameof(airtimeDataRetriever));
            this.fastPassDataRetriever = fastPassDataRetriever ?? throw new ArgumentNullException(nameof(fastPassDataRetriever));

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
                    // shoehorn FastPass into Centova response
                    var fastPassData = fastPassDataRetriever.GetFastPassData();
                    response.Centova = new Centova
                    {
                        Album = fastPassData.Album,
                        Artist = fastPassData.Artist,
                        Title = fastPassData.Title
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
