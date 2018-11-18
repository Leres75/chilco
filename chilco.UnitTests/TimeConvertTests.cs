using Xunit;

namespace chilco.UnitTests
{
    public class TimeConvertTests
    {
        [Fact]
        public static void MinToMillis_1_60000()
        {
            Assert.Equal(60000, TimeConvert.MinutesToMillis(1));
        }

        [Fact]
        public static void MillisToSeconds_1000_1()
        {
            Assert.Equal(1, TimeConvert.MillisToSeconds(1000));
        }

        [Fact]
        public static void SecondsToMillis_1_1000()
        {
            Assert.Equal(1000, TimeConvert.SecondsToMillis(1));
        }

        [Fact]
        public static void SecondsToMinutes_60_1()
        {
            Assert.Equal(1, TimeConvert.SecondsToMinutes(60));
        }

        [Fact]
        public static void MinutesToSeconds_1_60()
        {
            Assert.Equal(60, TimeConvert.MinutesToSeconds(1));
        }

        [Fact]
        public static void MinutesToHours_60_1()
        {
            Assert.Equal(1, TimeConvert.MinutesToHours(60));
        }

        [Fact]
        public static void HoursToMinutes_1_60()
        {
            Assert.Equal(60, TimeConvert.HoursToMinutes(1));
        }

        [Fact]
        public static void InputIsValid_1_True()
        {
            Assert.True(TimeConvert.InputIsValid(1));
        }

        [Fact]
        public static void InputIsValid_negative1_False()
        {
            Assert.False(TimeConvert.InputIsValid(-1));
        }
    }
}