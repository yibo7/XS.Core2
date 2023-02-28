
namespace Mustang
{
    using System;

    /// <summary>
    /// Extensions for date and time.
    /// </summary>
    public static class UnixTimestampExtensions
    {
        /// <summary>
        /// The JavaScript date is based on a time value that is milliseconds since midnight 01 January, 1970 UTC.
        /// </summary>
        public static readonly DateTime Epoch = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
        /// <summary>
        /// Gets UNIT timestamp, in seconds, for the specified date time.
        /// </summary>
        public static long GetUnixTimestamp(this DateTime d)
        {
            switch (d.Kind)
            {
                case DateTimeKind.Utc:
                    return (long)(d - Epoch).TotalSeconds;

                case DateTimeKind.Local:
                    return (long)(d.ToUniversalTime() - Epoch).TotalSeconds;

                default:
                    throw new ArgumentException($"Invalid kind of the specified date time: ${ d.ToString("o") }");
            }
        }

        /// <summary>
        /// Gets UNIT timestamp, in milliseconds, for the specified date time.
        /// </summary>
        public static long GetUnixTimestampInMS(this DateTime d)
        {
            switch (d.Kind)
            {
                case DateTimeKind.Utc:
                    return (long)(d - Epoch).TotalMilliseconds;

                case DateTimeKind.Local:
                    return (long)(d.ToUniversalTime() - Epoch).TotalMilliseconds;

                default:
                    throw new ArgumentException($"Invalid kind of the specified date time: ${ d.ToString("o") }");
            }
        }

        public static DateTime FromUnixTimestamp(this long seconds)
        {
            return Epoch.AddSeconds(seconds).ToLocalTime();
        }
    }
}
