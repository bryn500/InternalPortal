namespace InternalPortal.Web.Extensions
{
    public static class DateTimeExtension
    {
        public static string ToGdsString(this DateTime dateTime)
        {
            var gmt = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            DateTimeOffset timeInGmt = TimeZoneInfo.ConvertTime(dateTime, gmt);
            return timeInGmt.ToString("d MMMM yyyy"); //4 June 2017
        }
    }
}
