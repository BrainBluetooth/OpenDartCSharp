using System;

namespace OpenDARTCSharp
{
    internal static class Util
    {
        public static bool IsZIP(this byte[] bytes)
        {
            return bytes.Length >= 4 &&
                bytes[0] == 'P' &&
                bytes[1] == 'K' &&
                bytes[2] == 3 &&
                bytes[3] == 4;
        }

        public static string GetNextFileName()
        {
            return DateTime.Now.Ticks.ToString();
        }

        public static DateTime Parse_yyyyMMdd(this string str)
        {
            return new DateTime(
                year:
                    str[0] * 1000 + str[1] * 100 + str[2] * 10 + str[3]
                    - '0' * 1111,
                month:
                    str[4] * 10 + str[5]
                    - '0' * 11
                ,
                day:
                    str[6] * 10 + str[7]
                    - '0' * 11
                );
        }
    }
}