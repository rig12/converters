using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converters.Extensions
{
    internal static class Common
    {
        public static bool IsSimple(this Type type)
        {
            return type.IsPrimitive || type.Equals(typeof(string));
        }

        public static DateTime ConvertFromUnixTimestamp(this double timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);
        }

        public static double ConvertToUnixTimestamp(this DateTime date)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return Math.Floor((date.ToUniversalTime() - dateTime).TotalSeconds);
        }
    }
}
