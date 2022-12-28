namespace InternalPortal.Web.Extensions
{
    public static class DateTimeExtension
    {
        public static string ToGdsString(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                var date = TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                return GetGdsDate(date);
            }

            return GetGdsDate(dateTime);
        }

        private static string GetGdsDate(DateTimeOffset dateTime)
        {
            return dateTime.ToString("d MMMM yyyy"); //4 June 2017
        }
    }
}
