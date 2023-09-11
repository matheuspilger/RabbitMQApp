using System.Globalization;

namespace RabbitMQApp.Domain.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime? Parse(this string dateString)
        {
            if (dateString.Length < 14) return null;
            //20230909083515

            string format = "yyyyMMddHHmmss";
            return DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
        }
    }
}
