using System;

namespace ProtoPrimitives.NET.Tests.Temporal
{
    internal static class TemporalExtensions
    {
        public enum TimeMagnitude
        {
            Millisecond = 1,
            Second = 2,
            Minute = 3,
            Hour = 4,
            Day = 5,
            Month = 6,
            Year = 7
        }

        internal static DateTimeOffset FromMagnitude(this DateTimeOffset offset, TimeMagnitude timeMagnitude, int delta)
            => timeMagnitude switch
            {
                TimeMagnitude.Millisecond => offset.AddMilliseconds(delta),
                TimeMagnitude.Second => offset.AddSeconds(delta),
                TimeMagnitude.Minute => offset.AddMinutes(delta),
                TimeMagnitude.Day => offset.AddDays(delta),
                TimeMagnitude.Hour => offset.AddHours(delta),
                TimeMagnitude.Month => offset.AddMonths(delta),
                TimeMagnitude.Year => offset.AddYears(delta),
                _ => throw new ArgumentOutOfRangeException(nameof(timeMagnitude), timeMagnitude, "Unknown time magnitude.")
            };
    }
}
