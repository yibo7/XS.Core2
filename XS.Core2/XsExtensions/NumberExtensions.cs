
namespace XS.Core2.XsExtensions
{
    using System; 

    public enum NumberRoundBehavior
    {
        Floor = 1,
        Round,
        Ceiling
    }

    public static class NumberExtensions
    {
        /// <summary>
        /// The maximum number of fractional digits for amount/contract price numbers.
        /// </summary>
        public const int MaxPrecision = 8;

        /// <summary>
        /// Make sure NumberScale is equal to 10^8 (defined by MaxFractionalDigits).
        /// </summary>
        public const long NumberScale = 100000000L;

        /// <summary>
        /// Make sure AmountScale is equal to 10^8 (defined by MaxFractionalDigits).
        /// </summary>
        //public const decimal MinTickerPrice = 1m / NumberScale;
        /// <summary>
        /// Round the specified floating-point numbers.
        /// </summary>
        public static long RoundToInteger(this decimal d, int numDecimalPoints, NumberRoundBehavior behavior)
        {
            if (numDecimalPoints < 0)
                throw new ArgumentOutOfRangeException(nameof(numDecimalPoints));

            if (numDecimalPoints >= MaxPrecision)
                return (long)RoundNumber(d * NumberScale, behavior);

            if (numDecimalPoints <= 0)
                return (long)RoundNumber(d, behavior) * NumberScale;

            d = RoundNumber(d * (decimal)Math.Pow(10, numDecimalPoints), behavior);
            return (long)Math.Round(d * (decimal)Math.Pow(10, MaxPrecision - numDecimalPoints));
        }

        public static decimal RoundToPrecision(this decimal d, int precision)
        {
            if (precision < 0 || precision > MaxPrecision)
                throw new ArgumentOutOfRangeException(nameof(precision));

            return Math.Round(d, precision, MidpointRounding.AwayFromZero);
        }

        public static decimal FloorToPrecision(this decimal d, int precision)
        {
            if (precision < 0 || precision > MaxPrecision)
                throw new ArgumentOutOfRangeException(nameof(precision));

            if (precision == 0)
                return Math.Floor(d);

            long scale = (long)Math.Pow(10, precision);
            return Math.Floor(d * scale) / scale;
        }

        public static decimal CeilingToPrecision(this decimal d, int precision)
        {
            if (precision < 0 || precision > MaxPrecision)
                throw new ArgumentOutOfRangeException(nameof(precision));

            if (precision == 0)
                return Math.Ceiling(d);

            long scale = (long)Math.Pow(10, precision);
            return Math.Ceiling(d * scale) / scale;
        }

        private static decimal RoundNumber(decimal d, NumberRoundBehavior behavior)
        {
            switch (behavior)
            {
                case NumberRoundBehavior.Floor: return Math.Floor(d);
                case NumberRoundBehavior.Round: return Math.Round(d);
                case NumberRoundBehavior.Ceiling: return Math.Ceiling(d);

                default:
                    throw new ArgumentOutOfRangeException(nameof(behavior), behavior, $"Unsupported round behavior: {behavior}.");
            }
        }
    }
}
