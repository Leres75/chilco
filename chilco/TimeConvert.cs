namespace chilco
{
    public static class TimeConvert
    {
        public static double MillisToSeconds(double millis) => millis / 1000;

        public static double SecondsToMillis(double seconds) => seconds * 1000;

        public static double SecondsToMinutes(double seconds) => seconds / 60;

        public static double MinutesToSeconds(double minutes) => minutes * 60;

        public static double MinutesToHours(double minutes) => minutes / 60;

        public static double HoursToMinutes(double hours) => hours * 60;

        /// <summary>
        ///Converts the given value of minutes into milliseconds
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns>given minutes in milliseconds</returns>
        public static double MinutesToMillis(double minutes) => minutes * 60_000;
    }
}