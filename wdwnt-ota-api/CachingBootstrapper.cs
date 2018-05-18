using Nancy;
using Nancy.Bootstrapper;
using Nancy.Json;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using wdwnt_ota_api.Utilities.Extensions;

namespace wdwnt_ota_api
{
    public class CachingBootstrapper : DefaultNancyBootstrapper
    {
        private const int CACHE_SECONDS = 30;

        private readonly Dictionary<string, Tuple<DateTime, Response, int>> cachedResponses = new Dictionary<string, Tuple<DateTime, Response, int>>();

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            pipelines.BeforeRequest += CheckCache;

            pipelines.AfterRequest += SetupAfterRequest;

            StaticConfiguration.DisableErrorTraces = false;
            JsonSettings.MaxJsonLength = int.MaxValue;
        }

        public Response CheckCache(NancyContext context)
        {
            if (cachedResponses.TryGetValue(GetContextRequestPath(context), out Tuple<DateTime, Response, int> cacheEntry))
            {
                if (cacheEntry.Item1.AddSeconds(cacheEntry.Item3) > DateTime.Now)
                {
                    return cacheEntry.Item2;
                }
            }

            return null;
        }

        public void SetupAfterRequest(NancyContext context)
        {
            AddResponseHeadersToContext(context);
            GetContextResponse(context);
        }

        private void AddResponseHeadersToContext(NancyContext context)
        {
            if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            }

            if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Headers"))
            {
                context.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, Content-Type, Accept");
            }
        }

        private string GetContextRequestPath(NancyContext context)
        {
            return context.Request.Path.Replace(".json", string.Empty)
                .Replace(".xml", string.Empty);
        }

        private void GetContextResponse(NancyContext context)
        {
            if (context.Response.StatusCode != HttpStatusCode.OK)
            {
                return;
            }

            if (!context.Items.TryGetValue(ContextExtensions.OUTPUT_CACHE_TIME_KEY, out object cacheSecondsObject))
            {
                return;
            }

            if (!int.TryParse(cacheSecondsObject.ToString(), out int cacheSeconds))
            {
                return;
            }

            var cachedResponse = new CachedResponse(context.Response);

            cachedResponses[GetContextRequestPath(context)] = new Tuple<DateTime, Response, int>(DateTime.Now, cachedResponse, cacheSeconds);

            context.Response = cachedResponse;
        }
    }
}