using System;
using Xunit;
using chilco;

namespace chilco.UnitTests
{
    public class TimeConvertTests
    {
        [Fact]
        public static void MinToMillis_ValEquals1_Returns60000()
        {
                Assert.Equal(60000, TimeConvert.MinutesToMillis(1));
        }

        [Fact]
        public static void MillisToSeconds_ValEquals1000_Returns1()
        {
            Assert.Equal(1, TimeConvert.MillisToSeconds(1000));
        }

        [Fact]
        public static void SecondsToMillis_ValEquals1_Returns1000()
        {
            Assert.Equal(1000, TimeConvert.SecondsToMillis(1));
        }

        [Fact]
        public static void SecondsToMinutes_ValEquals60_Returns1()
        {
            Assert.Equal(1, TimeConvert.SecondsToMinutes(60));
        }

        [Fact]
        public static void MinutesToSeconds_ValEquals1_Returns60()
        {
            Assert.Equal(60, TimeConvert.MinutesToSeconds(1));
        }

        [Fact]
        public static void MinutesToHours_ValEquals60_Returns1()
        {
            Assert.Equal(1, TimeConvert.MinutesToHours(60));
        }

        [Fact]
        public static void HoursToMinutes_ValEquals1_Returns60()
        {
            Assert.Equal(60, TimeConvert.HoursToMinutes(1));
        }
    }
}
