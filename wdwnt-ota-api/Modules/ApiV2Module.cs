using System;
using System.Configuration;
using Nancy;
using WdwntOtaApi.Models;
using wdwnt_ota_api.BusinessLogic;

namespace wdwnt_ota_api.Modules
{
    public class ApiV2Module : BaseApiModule
    {
        private readonly ICentovaDataRetriever centovaDataRetriever;
        private readonly IAirtimeDataRetriever airtimeDataRetriever;

        public ApiV2Module(ICentovaDataRetriever centovaDataRetriever, IAirtimeDataRetriever airtimeDataRetriever) : base("v2")
        {
            this.centovaDataRetriever = centovaDataRetriever ?? throw new ArgumentNullException(nameof(centovaDataRetriever));
            this.airtimeDataRetriever = airtimeDataRetriever ?? throw new ArgumentNullException(nameof(airtimeDataRetriever));

            Get["/info"] = _ =>
            {
                var otaStreamUrl = ConfigurationManager.AppSettings["AirtimeStreamUrl"];
                var response = new ApiV2Result
                {
                    Ota_stream_url = !Request.Headers.UserAgent.Contains("Android") ?
                                        $"{otaStreamUrl}.m3u" :
                                        otaStreamUrl
                };

                response.Suggest_radio = DateTimeCalculator.NowEst.DayOfWeek == DayOfWeek.Wednesday &&
                                        (DateTimeCalculator.NowEst.Hour == 21 || DateTimeCalculator.NowEst.Hour == 22);

                try
                {
                    response.Centova = this.centovaDataRetriever.GetCentovaData();
                    response.Airtime = this.airtimeDataRetriever.GetAirtimeData();
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