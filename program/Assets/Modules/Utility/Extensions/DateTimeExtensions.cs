using System;
using System.Globalization;

namespace Utility {
    public static class DateTimeExtensions {
        public const string ExactFormat = "yyyy/MM/dd HH:mm:ss";

        public static DateTime ToExactTime(this string dateStr) => DateTime.ParseExact(dateStr, ExactFormat, CultureInfo.InvariantCulture);

        public static string ToExactString(this DateTime dateTime) => dateTime.ToString(ExactFormat);
    }
}