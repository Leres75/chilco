namespace chilco
{
    public static class TimeConvert
    {


        public static double MillisToSeconds(double millis)
        {
            if (InputIsValid(millis))
            {
                return millis / 1000;
            }
            return -1;
        }

        public static double SecondsToMillis(double seconds)
        {
            if (InputIsValid(seconds))
            {
                return seconds * 1000;
            }
            return -1;
        }

        public static double SecondsToMinutes(double seconds)
        {
            if (InputIsValid(seconds))
            {
                return seconds / 60;
            }
            return -1;
        }

        public static double MinutesToSeconds(double minutes)
        {
            if (InputIsValid(minutes))
            {
                return minutes * 60;
            }
            return -1;
        }

        public static double MinutesToHours(double minutes)
        {
            if (InputIsValid(minutes))
            {
                return minutes / 60;
            }
            return -1;
        }

        public static double HoursToMinutes(double hours)
        {
            if (InputIsValid(hours))
            {
                return hours * 60;
            }
            return -1;
        }

        /// <summary>
        ///Converts the given value of minutes doubleo milliseconds
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns>given minutes in milliseconds</returns>
        public static double MinutesToMillis(double minutes)
        {
            if (InputIsValid(minutes))
            {
                return minutes * 60_000;
            }
            return -1;
        }

        public static bool InputIsValid(double input)
        {
            return !(input <= 0);
        }
    }
}