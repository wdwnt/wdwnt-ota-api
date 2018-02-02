using System;
using System.Configuration;
using Nancy;
using WdwntOtaApi.Models;
using wdwnt_ota_api.BusinessLogic;

namespace wdwnt_ota_api.Modules
{
    public class ApiModule : BaseApiModule
    {
        private readonly ICentovaDataRetriever centovaDataRetriever;

        public ApiModule(ICentovaDataRetriever centovaDataRetriever) : base("v1")
        {
            this.centovaDataRetriever = centovaDataRetriever ?? throw new ArgumentNullException(nameof(centovaDataRetriever));

            Get["/info"] = _ =>
            {
                var otaStreamUrl = ConfigurationManager.AppSettings["OTAStreamUrl"];
                var response = new ApiV1Result
                {
                    Ota_stream_url = !Request.Headers.UserAgent.Contains("Android") ?
                                        $"{otaStreamUrl}.m3u" :
                                        otaStreamUrl
                };

                response.Suggest_radio = false;

                try
                {
                    response.Centova = this.centovaDataRetriever.GetCentovaData();
                }
                catch (Exception e)
                {
                    response.Centova = null;
                    response.Error = e.Message;
                }

                return Response.AsJson((object)response);
            };
        }
    }
}
