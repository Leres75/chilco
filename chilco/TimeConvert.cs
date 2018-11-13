using System;
using System.Collections.Generic;
using System.Text;

namespace chilco
{
    public static class TimeConvert
    {
        public static long MinToMillis(int minutes)
        {
            return minutes * 60_000;
        }

        public static int MillisToSeconds(long millis)
        {
            return (int)millis / 60_000;
        }

        public static int MillisToHours(long millis)
        {
            return (int)millis / 3_600_000;
        }
    }
}
