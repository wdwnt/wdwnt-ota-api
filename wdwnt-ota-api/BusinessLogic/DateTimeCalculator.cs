using System;

namespace wdwnt_ota_api.BusinessLogic
{
    public class DateTimeCalculator
    {
        public static DateTime NowEst => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
    }
}
