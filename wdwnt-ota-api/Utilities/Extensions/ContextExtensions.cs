using Nancy;

namespace wdwnt_ota_api.Utilities.Extensions
{
    public static class ContextExtensions
    {
        public const string OUTPUT_CACHE_TIME_KEY = "OUTPUT_CACHE_TIME";
        private const int DefaultCacheTimeInSeconds = 30;

        public static void EnableOutputCache(this NancyContext context, int seconds)
        {
            context.Items[OUTPUT_CACHE_TIME_KEY] = seconds;
        }

        public static void EnableDefaultOutputCache(this NancyContext context)
        {
            EnableOutputCache(context, DefaultCacheTimeInSeconds);
        }

        public static void DisableOutputCache(this NancyContext context)
        {
            context.Items.Remove(OUTPUT_CACHE_TIME_KEY);
        }
    }
}